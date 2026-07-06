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

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public List<ProductDto> GetAllProducts()
        {
            var products = _productRepository.GetAllProducts().ToList();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public ProductDto? GetProductById(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return null;
            return _mapper.Map<ProductDto>(product);
        }

        public ProductDto CreateProduct(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            var createdProduct = _productRepository.CreateProduct(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public ProductDto? UpdateProduct(int id, UpdateProductDto dto)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return null;

            _mapper.Map(dto, product);
            _productRepository.UpdateProduct(product);
            
            return _mapper.Map<ProductDto>(product);
        }

        public bool DeleteProduct(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return false;

            _productRepository.DeleteProduct(product);
            return true;
        }
    }
}
