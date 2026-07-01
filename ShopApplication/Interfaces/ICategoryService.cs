using System.Collections.Generic;
using ShopDomain.Models;

namespace ShopApplication.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
    }
}
