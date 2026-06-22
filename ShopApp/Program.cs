
namespace ShopApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<ShopApp.Interfaces.IProductService, ShopApp.Services.ProductService>();
            builder.Services.AddSingleton<ShopApp.Interfaces.ICategoryService, ShopApp.Services.CategoryService>();

            builder.Services.AddControllers();

            var app = builder.Build();


            app.MapControllers();

            app.Run();
        }
    }
}
