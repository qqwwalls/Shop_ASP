using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Application.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<User>? RegisterUserAsync(User user, string hash);
        Task<bool> IsExistEmailAsync(string email);
    }
}
