using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Shop.Domain.Models;

namespace ShopApp.Middlewares
{
    public class UserCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public UserCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Перевіряємо шлях і метод
            if (context.Request.Path.Value != null && 
                context.Request.Path.Value.ToLower() == "/api/user/register" && 
                HttpMethods.IsPost(context.Request.Method))
            {
                // Дозволяємо читати тіло запиту кілька разів
                context.Request.EnableBuffering();
                
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var bodyString = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Скидаємо позицію для контролера

                User user = null;
                try 
                {
                    user = JsonSerializer.Deserialize<User>(bodyString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch
                {
                    // Ігноруємо помилки парсингу (наприклад, пусте тіло)
                }

                // Перевіряємо умови
                if (user != null && user.Login == "admin" && user.Id == 1)
                {
                    // Пропускаємо запит у контролер
                    await _next(context);
                }
                else
                {
                    // Видаємо відповідь про відсутність авторизації
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\n\t\"message\": \"No authorization\"\n}");
                }
            }
            else
            {
                // Інші запити просто пропускаємо далі
                await _next(context);
            }
        }
    }
}
