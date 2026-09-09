using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Services;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        private void SetRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Append(
                "refreshToken",
                refreshToken,
                new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
                });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(dto, cancellationToken);

            if (result.User == null)
            {
                return BadRequest("User with this email already exists.");
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenCookie(result.RefreshToken);
            }

            return Ok(new { User = result.User, Token = result.AccessToken });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserCreateDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAdminAsync(dto, cancellationToken);

            if (result.User == null)
            {
                return BadRequest("User with this email already exists.");
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenCookie(result.RefreshToken);
            }

            return Ok(new { User = result.User, Token = result.AccessToken });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(dto, cancellationToken);

            if (result.User == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenCookie(result.RefreshToken);
            }

            return Ok(new { User = result.User, Token = result.AccessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token provided.");
            }

            var newAccessToken = await _authService.RefreshAccessTokenAsync(refreshToken, cancellationToken);

            if (string.IsNullOrEmpty(newAccessToken))
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            return Ok(new { Token = newAccessToken });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.GeneratePasswordResetTokenAsync(dto, cancellationToken);

            if (token == null)
            {
                // To prevent email enumeration attacks, we usually return Ok even if the email wasn't found
                return Ok(new { Message = "If that email address is in our database, we will send you an email to reset your password." });
            }

            // In a real app, send an email here. For now, just return it so it can be tested in Swagger.
            return Ok(new { 
                Message = "In a real app, an email would be sent. For testing purposes, here is the token.",
                ResetToken = token 
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _authService.ResetPasswordAsync(dto, cancellationToken);

            if (!success)
            {
                return BadRequest("Invalid or expired reset token.");
            }

            return Ok(new { Message = "Password has been successfully reset." });
        }
    }
}
