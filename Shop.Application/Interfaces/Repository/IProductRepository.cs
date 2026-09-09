using System.Collections.Generic;
using Shop.Domain.Models;

namespace Shop.Application.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(System.Threading.CancellationToken cancellationToken = default);
        Task<Product?> GetProductByIdAsync(int id, System.Threading.CancellationToken cancellationToken = default);
        Task<Product> CreateProductAsync(Product product, System.Threading.CancellationToken cancellationToken = default);
        Task UpdateProductAsync(Product product, System.Threading.CancellationToken cancellationToken = default);
        Task DeleteProductAsync(Product product, System.Threading.CancellationToken cancellationToken = default);
    }
}
