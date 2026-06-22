using System.Collections.Generic;
using ShopDomain.Models;

namespace ShopApp.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
    }
}
