using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Application.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<User>? RegisterUserAsync(User user, string hash);
        Task<bool> IsExistEmailAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByResetTokenAsync(string token);
        Task UpdateUserAsync(User user);
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
    }
}
