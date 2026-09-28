using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Services;
using Shop.Application.Helpers;
using Shop.Infrastructure.Repositories;
using Shop.Api.Interfaces;
using Shop.Api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Shop.Application.Validators.Category;

namespace Shop.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHashHelper, HashHelper>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IImageService, ImageService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
        }

        public static void ConfigureValidators(this IServiceCollection services)
        {
            //======================VALIDATORS=================
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
        }
    }
}
