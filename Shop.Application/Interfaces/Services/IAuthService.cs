using System.Threading.Tasks;
using Shop.Application.DTOs.UserDTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAsync(UserCreateDTO dto, CancellationToken cancellationToken = default);
        Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAdminAsync(UserCreateDTO dto, CancellationToken cancellationToken = default);
        Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> LoginAsync(UserLoginDTO dto, CancellationToken cancellationToken = default);
        Task<string?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<string?> GeneratePasswordResetTokenAsync(ForgotPasswordDTO dto, CancellationToken cancellationToken = default);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto, CancellationToken cancellationToken = default);
    }
}
