using Shop.Application.DTOs.UserDTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface IJWTService
    {
        public string GenerateAccessToken(UserLoginDTO userLoginDto, string role);
        public (string, int) GenerateRefreshToken();
    }
}
