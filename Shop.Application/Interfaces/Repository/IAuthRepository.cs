using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Application.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<User>? RegisterUserAsync(User user, string hash, System.Threading.CancellationToken cancellationToken = default);
        Task<bool> IsExistEmailAsync(string email, System.Threading.CancellationToken cancellationToken = default);
        Task<User?> GetUserByEmailAsync(string email, System.Threading.CancellationToken cancellationToken = default);
        Task<User?> GetUserByResetTokenAsync(string token, System.Threading.CancellationToken cancellationToken = default);
        Task UpdateUserAsync(User user, System.Threading.CancellationToken cancellationToken = default);
        Task SaveRefreshTokenAsync(RefreshToken token, System.Threading.CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetRefreshTokenAsync(string token, System.Threading.CancellationToken cancellationToken = default);
    }
}
