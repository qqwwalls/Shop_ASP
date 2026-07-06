using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        List<CategoryReadDTO> GetAllCategories();
        Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
        Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
    }
}
