using Microsoft.EntityFrameworkCore;
using ShopInfrastructure.Data;

namespace ShopApi
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

            builder.Services.AddScoped<ShopApplication.Interfaces.IProductService, ShopApplication.Services.ProductService>();
            builder.Services.AddSingleton<ShopApplication.Interfaces.ICategoryService, ShopApplication.Services.CategoryService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseMiddleware<ShopApi.Middlewares.RequestTimerMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
