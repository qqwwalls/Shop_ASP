using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync(System.Threading.CancellationToken cancellationToken = default);
        Task<ProductDto?> GetProductByIdAsync(int id, System.Threading.CancellationToken cancellationToken = default);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto, System.Threading.CancellationToken cancellationToken = default);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto, System.Threading.CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(int id, System.Threading.CancellationToken cancellationToken = default);
    }
}
