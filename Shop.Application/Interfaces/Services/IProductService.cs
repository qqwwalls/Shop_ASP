using System.Collections.Generic;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces.Services
{
    public interface IProductService
    {
        List<ProductDto> GetAllProducts();
        ProductDto? GetProductById(int id);
        ProductDto CreateProduct(CreateProductDto dto);
        ProductDto? UpdateProduct(int id, UpdateProductDto dto);
        bool DeleteProduct(int id);
    }
}
