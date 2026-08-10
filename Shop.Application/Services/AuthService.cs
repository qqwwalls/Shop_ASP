using System.Threading.Tasks;
using AutoMapper;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Models;

namespace Shop.Application.Services
{
    public class AuthService(IMapper _mapper, IAuthRepository _repository, IHashHelper _hashHelper, IJWTService _jwtService) : IAuthService
    {
        public async Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAsync(UserCreateDTO dto)
        {
            var isExist = await _repository.IsExistEmailAsync(dto.Email);
            if (!isExist)
            {
                var hash = _hashHelper.Hash(dto.Password);
                var user = _mapper.Map<User>(dto);
                var token = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                var registerUser = await _repository.RegisterUserAsync(user, hash);
                
                if (registerUser != null)
                {
                    var (refreshTokenStr, days) = _jwtService.GenerateRefreshToken();
                    var refreshTokenEntity = new RefreshToken
                    {
                        Token = refreshTokenStr,
                        ExpiresAt = System.DateTime.UtcNow.AddDays(days),
                        IsRevoked = false,
                        UserId = registerUser.Id
                    };
                    await _repository.SaveRefreshTokenAsync(refreshTokenEntity);

                    return (_mapper.Map<UserReadDTO>(registerUser), token, refreshTokenStr);
                }
            }
            return (null, null, null);
        }

        public async Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAdminAsync(UserCreateDTO dto)
        {
            var isExist = await _repository.IsExistEmailAsync(dto.Email);
            if (!isExist)
            {
                var hash = _hashHelper.Hash(dto.Password);
                var user = _mapper.Map<User>(dto);
                user.Role = Shop.Domain.Enums.UserRole.Admin; // Set role to Admin
                
                var token = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                var registerUser = await _repository.RegisterUserAsync(user, hash);
                
                if (registerUser != null)
                {
                    var (refreshTokenStr, days) = _jwtService.GenerateRefreshToken();
                    var refreshTokenEntity = new RefreshToken
                    {
                        Token = refreshTokenStr,
                        ExpiresAt = System.DateTime.UtcNow.AddDays(days),
                        IsRevoked = false,
                        UserId = registerUser.Id
                    };
                    await _repository.SaveRefreshTokenAsync(refreshTokenEntity);

                    return (_mapper.Map<UserReadDTO>(registerUser), token, refreshTokenStr);
                }
            }
            return (null, null, null);
        }

        public async Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> LoginAsync(UserLoginDTO dto)
        {
            var user = await _repository.GetUserByEmailAsync(dto.Email);
            if (user != null && _hashHelper.IsValidPassword(dto.Password, user.PasswordHash))
            {
                var token = _jwtService.GenerateAccessToken(dto, user.Role.ToString());
                var (refreshTokenStr, days) = _jwtService.GenerateRefreshToken();
                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshTokenStr,
                    ExpiresAt = System.DateTime.UtcNow.AddDays(days),
                    IsRevoked = false,
                    UserId = user.Id
                };
                await _repository.SaveRefreshTokenAsync(refreshTokenEntity);

                return (_mapper.Map<UserReadDTO>(user), token, refreshTokenStr);
            }
            return (null, null, null);
        }

        public async Task<string?> RefreshAccessTokenAsync(string refreshToken)
        {
            var tokenEntity = await _repository.GetRefreshTokenAsync(refreshToken);

            if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiresAt <= System.DateTime.UtcNow)
            {
                return null;
            }

            var loginDto = new UserLoginDTO
            {
                Email = tokenEntity.User.Email,
                Password = "" // Not needed for GenerateAccessToken since we only use Email
            };

            return _jwtService.GenerateAccessToken(loginDto, tokenEntity.User.Role.ToString());
        }
    }
}
