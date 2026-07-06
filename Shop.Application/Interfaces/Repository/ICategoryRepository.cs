using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Application.Interfaces.Repository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        Category CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Task<int?> AddCategoryAsync(Category category);
        Task<Category?> GetCategoryByIdAsync(int id);
    }
}
