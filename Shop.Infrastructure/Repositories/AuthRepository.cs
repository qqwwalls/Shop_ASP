using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Domain.Models;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories
{
    public class AuthRepository(ShopDbContext _context) : IAuthRepository
    {
        public async Task<bool> IsExistEmailAsync(string email)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
            if(userFromDb == null)
                return false;
            return true;
        }

        public async Task<User>? RegisterUserAsync(User user, string hash)
        {
            user.PasswordHash = hash;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return await _context.Users.FirstOrDefaultAsync(us => (us.Email == user.Email && us.PasswordHash == user.PasswordHash));
            /*
             * TODO:
             1) Перевірити чи немає вже у БД такого email
             2) Захешувати пароль
             3) Додати користувача у БД
             4) Зробити токен, скоріше за все не тут будемо робити
             */
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);
        }
    }
}
