using System.Threading.Tasks;
using Shop.Application.DTOs.UserDTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserReadDTO?> RegisterAsync(UserCreateDTO dto);
    }
}
