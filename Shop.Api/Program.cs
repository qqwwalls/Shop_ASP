using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Services;
using Shop.Application.Helpers;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Repositories;

namespace Shop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IHashHelper, HashHelper>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            builder.Services.AddScoped<Shop.Api.Interfaces.IImageService, Shop.Api.Services.ImageService>();

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
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseStaticFiles();
            app.UseMiddleware<Shop.Api.Middlewares.RequestTimerMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
