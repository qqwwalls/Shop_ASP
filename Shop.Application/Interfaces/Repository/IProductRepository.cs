using System.Collections.Generic;
using Shop.Domain.Models;

namespace Shop.Application.Interfaces.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(int id);
        Product CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
