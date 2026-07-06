using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Application.Services;
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

            //--------------REPOSITORIES
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<Shop.Api.Interfaces.IImageService, Shop.Api.Services.ImageService>();

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<Shop.Application.Mappings.MappingProfile>());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => 
            {
                c.EnableAnnotations();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseMiddleware<Shop.Api.Middlewares.RequestTimerMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
