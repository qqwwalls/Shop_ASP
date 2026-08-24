using System.Collections.Generic;
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

        public List<ProductDto> GetAllProducts()
        {
            var cachedProducts = _cachingService.GetAsync<List<ProductDto>>(CacheKey).GetAwaiter().GetResult();
            if (cachedProducts != null)
            {
                return cachedProducts;
            }

            var products = _productRepository.GetAllProducts().ToList();
            var dtos = _mapper.Map<List<ProductDto>>(products);
            
            _cachingService.SetAsync(CacheKey, dtos).GetAwaiter().GetResult();
            return dtos;
        }

        public ProductDto? GetProductById(int id)
        {
            var cacheKey = $"Product_{id}";
            var cachedProduct = _cachingService.GetAsync<ProductDto>(cacheKey).GetAwaiter().GetResult();
            
            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            var product = _productRepository.GetProductById(id);
            if (product == null) return null;
            
            var dto = _mapper.Map<ProductDto>(product);
            _cachingService.SetAsync(cacheKey, dto).GetAwaiter().GetResult();
            
            return dto;
        }

        public ProductDto CreateProduct(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            var createdProduct = _productRepository.CreateProduct(product);
            
            _cachingService.RemoveAsync(CacheKey).GetAwaiter().GetResult();
            
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public ProductDto? UpdateProduct(int id, UpdateProductDto dto)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return null;

            _mapper.Map(dto, product);
            _productRepository.UpdateProduct(product);
            
            _cachingService.RemoveAsync(CacheKey).GetAwaiter().GetResult();
            
            return _mapper.Map<ProductDto>(product);
        }

        public bool DeleteProduct(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return false;

            _productRepository.DeleteProduct(product);
            
            _cachingService.RemoveAsync(CacheKey).GetAwaiter().GetResult();
            
            return true;
        }
    }
}
