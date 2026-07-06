using AutoMapper;
using Shop.Domain.Models;
using Shop.Application.DTOs;

namespace Shop.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category Mappings
            CreateMap<Category, CategoryReadDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // Product Mappings
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
