using System.Collections.Generic;
using ShopDomain.Models;

namespace ShopApplication.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
    }
}
