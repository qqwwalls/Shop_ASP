using System.Collections.Generic;
using ShopDomain.Models;

namespace ShopApp.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
    }
}
