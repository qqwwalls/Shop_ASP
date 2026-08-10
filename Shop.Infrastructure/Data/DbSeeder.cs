using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Interfaces.Helpers;
using Shop.Domain.Enums;
using Shop.Domain.Models;

namespace Shop.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
            var hashHelper = scope.ServiceProvider.GetRequiredService<IHashHelper>();

            // Ensure database is created/migrated
            await context.Database.MigrateAsync();

            // Check if any admin exists
            var adminExists = await context.Users.AnyAsync(u => u.Role == UserRole.Admin);

            if (!adminExists)
            {
                var adminUser = new User
                {
                    Email = "admin@shop.com",
                    PasswordHash = hashHelper.Hash("Admin_123!"),
                    Role = UserRole.Admin,
                    IsActive = true
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
