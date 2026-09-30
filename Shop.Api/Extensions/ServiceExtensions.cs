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
using Polly;
using System;

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

        public static void ConfigurePollyHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<ICurrencyService, Shop.Infrastructure.Services.CurrencyService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        private static Polly.IAsyncPolicy<System.Net.Http.HttpResponseMessage> GetRetryPolicy()
        {
            return Polly.Policy
                .Handle<System.Net.Http.HttpRequestException>()
                .OrResult<System.Net.Http.HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => System.TimeSpan.FromSeconds(System.Math.Pow(2, retryAttempt)), // 3 спроби, експоненційна затримка (2, 4, 8 сек)
                    (result, timeSpan, retryCount, context) =>
                    {
                        System.Console.WriteLine($"[POLLY] Retry {retryCount} after {timeSpan.TotalSeconds} seconds due to: {result.Exception?.Message ?? result.Result?.StatusCode.ToString()}");
                    });
        }

        private static Polly.IAsyncPolicy<System.Net.Http.HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Polly.Policy
                .Handle<System.Net.Http.HttpRequestException>()
                .OrResult<System.Net.Http.HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, System.TimeSpan.FromSeconds(30), 
                    onBreak: (result, timespan) => System.Console.WriteLine($"[POLLY] Circuit Breaker OPENED for {timespan.TotalSeconds} seconds!"),
                    onReset: () => System.Console.WriteLine("[POLLY] Circuit Breaker RESET (Closed) - Requests flowing again."),
                    onHalfOpen: () => System.Console.WriteLine("[POLLY] Circuit Breaker HALF-OPEN - Testing one request..."));
        }
    }
}
