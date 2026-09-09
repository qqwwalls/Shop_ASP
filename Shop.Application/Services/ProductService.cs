using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Shop.Domain.Models;
using Shop.Application.Interfaces.Services;
using Shop.Application.Interfaces.Repository;
using Shop.Application.DTOs;

namespace Shop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cachingService;
        private const string CacheKey = "Products";

        public ProductService(IProductRepository productRepository, IMapper mapper, ICachingService cachingService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cachingService = cachingService;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            var cachedProducts = await _cachingService.GetAsync<List<ProductDto>>(CacheKey, cancellationToken);
            if (cachedProducts != null)
            {
                return cachedProducts;
            }

            var products = await _productRepository.GetAllProductsAsync(cancellationToken);
            var dtos = _mapper.Map<List<ProductDto>>(products.ToList());
            
            await _cachingService.SetAsync(CacheKey, dtos, cancellationToken: cancellationToken);
            return dtos;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"Product_{id}";
            var cachedProduct = await _cachingService.GetAsync<ProductDto>(cacheKey, cancellationToken);
            
            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            var product = await _productRepository.GetProductByIdAsync(id, cancellationToken);
            if (product == null) return null;
            
            var dto = _mapper.Map<ProductDto>(product);
            await _cachingService.SetAsync(cacheKey, dto, cancellationToken: cancellationToken);
            
            return dto;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
        {
            var product = _mapper.Map<Product>(dto);
            var createdProduct = await _productRepository.CreateProductAsync(product, cancellationToken);
            
            await _cachingService.RemoveAsync(CacheKey, cancellationToken);
            
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetProductByIdAsync(id, cancellationToken);
            if (product == null) return null;

            _mapper.Map(dto, product);
            await _productRepository.UpdateProductAsync(product, cancellationToken);
            
            await _cachingService.RemoveAsync(CacheKey, cancellationToken);
            
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetProductByIdAsync(id, cancellationToken);
            if (product == null) return false;

            await _productRepository.DeleteProductAsync(product, cancellationToken);
            
            await _cachingService.RemoveAsync(CacheKey, cancellationToken);
            
            return true;
        }
    }
}
