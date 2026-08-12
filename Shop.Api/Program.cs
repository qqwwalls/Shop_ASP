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
using System.Text;
using System;

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

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IHashHelper, HashHelper>();
            builder.Services.AddScoped<IJWTService, JWTService>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            builder.Services.AddScoped<Shop.Api.Interfaces.IImageService, Shop.Api.Services.ImageService>();

            // ================= Caching =================
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ICachingService, Shop.Infrastructure.Services.MemoryCachingService>();

            // ================= AutoMapper =================
            builder.Services.AddAutoMapper(
                _ => { },
                typeof(Shop.Application.Mappings.CategoryProfile).Assembly
            );

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => 
            {
                c.EnableAnnotations();
            });

            // ================= CORS =================
            builder.Services.AddCors(options =>
            {
                // Політика для розробки (дозволяє все)
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
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
