using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Domain.Models;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories
{
    public class AuthRepository(ShopDbContext _context) : IAuthRepository
    {
        public async Task<bool> IsExistEmailAsync(string email, System.Threading.CancellationToken cancellationToken = default)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
            if(userFromDb == null)
                return false;
            return true;
        }

        public async Task<User>? RegisterUserAsync(User user, string hash, System.Threading.CancellationToken cancellationToken = default)
        {
            user.PasswordHash = hash;
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return await _context.Users.FirstOrDefaultAsync(us => (us.Email == user.Email && us.PasswordHash == user.PasswordHash), cancellationToken);
            /*
             * TODO:
             1) Перевірити чи немає вже у БД такого email
             2) Захешувати пароль
             3) Додати користувача у БД
             4) Зробити токен, скоріше за все не тут будемо робити
             */
        }

        public async Task<User?> GetUserByEmailAsync(string email, System.Threading.CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetUserByResetTokenAsync(string token, System.Threading.CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token, cancellationToken);
        }

        public async Task UpdateUserAsync(User user, System.Threading.CancellationToken cancellationToken = default)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token, System.Threading.CancellationToken cancellationToken = default)
        {
            await _context.RefreshTokens.AddAsync(token, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token, System.Threading.CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
        }
    }
}
