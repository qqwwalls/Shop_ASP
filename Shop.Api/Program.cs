using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Services;
using Shop.Application.Helpers;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Shop.Api.Extensions;
using Shop.Api.Middlewares;

namespace Shop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // ================= JWT Settings =================
            var jwtSettings = configuration.GetSection("Jwt").Get<Shop.Application.Settings.JwtSettings>()
                ?? throw new Exception("JWT settings not configured.");
            
            builder.Services.Configure<Shop.Application.Settings.JwtSettings>(configuration.GetSection("Jwt"));

            // ================= Authentication =================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //Правила перевірки токена
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.ConfigureApplicationServices();
            builder.Services.ConfigureRepositories();

            // ================= Caching =================
            // Configure Redis Cache
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            });

            // Register IConnectionMultiplexer for explicit Redis operations (like Lists)
            builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            {
                return StackExchange.Redis.ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            });

            // Register CachingService
            builder.Services.AddSingleton<Shop.Application.Interfaces.Services.ICachingService, Shop.Infrastructure.Services.RedisCachingService>();

            // ================= RabbitMQ & MongoDB =================
            builder.Services.Configure<Shop.Infrastructure.Configuration.RabbitMqSettings>(
                builder.Configuration.GetSection("RabbitMq")
            );
            builder.Services.Configure<Shop.Infrastructure.Configuration.MongoDbSettings>(
                builder.Configuration.GetSection("MongoDb")
            );
            builder.Services.AddScoped<Shop.Application.Interfaces.Services.IQueueService, Shop.Infrastructure.Services.RabbitMqService>();
            
            // Залишаємо старий читач для Users (якщо треба)
            builder.Services.AddHostedService<Shop.Api.Services.RabbitMqReaderService>();
            
            // Реєструємо новий читач для Orders (зберігає в MongoDB)
            builder.Services.AddHostedService<Shop.Api.Services.RabbitMqMongoOrderConsumer>();

            // ================= AutoMapper =================
            builder.Services.AddAutoMapper(
                _ => { },
                typeof(Shop.Application.Mappings.CategoryProfile).Assembly
            );

            // ================= MediatR =================
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Shop.Application.Queries.GetProductById.GetProductByIdQuery).Assembly));

            builder.Services.ConfigureValidators();

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddControllers(options => 
            {
                options.ModelValidatorProviders.Clear(); 
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => 
            {
                c.EnableAnnotations();
            });

            // ================= CORS =================
            builder.Services.AddCors(options =>
            {
                // Політика для розробки (дозволяє все, крім wildcard origin)
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.SetIsOriginAllowed(origin => true) // Дозволяє будь-який origin, але не використовує '*'
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); // КРИТИЧНО ДЛЯ httpOnly Cookies!
                });

                // Політика для продакшену (сувора) - як показувала викладачка
                // options.AddPolicy("ProductionPolicy", policy =>
                // {
                //     policy.WithOrigins("https://miy-magazin.com", "https://www.miy-magazin.com")
                //           .WithMethods("GET", "POST", "PUT", "DELETE")
                //           .WithHeaders("Content-Type", "Authorization");
                // });
            });

            var app = builder.Build();

            // ================= Exception Handling =================
            // Використовуємо новий глобальний обробник помилок
            app.UseExceptionHandler();

            // ================= Seeding =================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    Shop.Infrastructure.Data.DbSeeder.SeedAdminAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseStaticFiles();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<Shop.Api.Middlewares.RequestTimerMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
