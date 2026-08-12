using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryReadDTO>> GetAllCategoriesAsync();
        Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
        Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
    }
}
