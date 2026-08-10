using System.Threading.Tasks;
using Shop.Application.DTOs.UserDTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAsync(UserCreateDTO dto);
        Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAdminAsync(UserCreateDTO dto);
        Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> LoginAsync(UserLoginDTO dto);
        Task<string?> RefreshAccessTokenAsync(string refreshToken);
        Task<string?> GeneratePasswordResetTokenAsync(ForgotPasswordDTO dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);
    }
}
