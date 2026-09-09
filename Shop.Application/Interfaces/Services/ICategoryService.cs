using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryReadDTO>> GetAllCategoriesAsync(System.Threading.CancellationToken cancellationToken = default);
        Task<int?> CreateCategoryAsync(CategoryCreateDTO dto, System.Threading.CancellationToken cancellationToken = default);
        Task<CategoryReadDTO?> GetCategoryByIdAsync(int id, System.Threading.CancellationToken cancellationToken = default);
    }
}
