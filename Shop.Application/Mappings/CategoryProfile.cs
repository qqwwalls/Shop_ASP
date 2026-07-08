using System.Linq;
using AutoMapper;
using Shop.Domain.Models;
using Shop.Application.DTOs;

namespace Shop.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Category Mappings
            CreateMap<CategoryCreateDTO, Category>();

            CreateMap<Category, CategoryReadDTO>()
                .ForMember(dest => dest.Products,
                    opt => opt.MapFrom(src => src.Products.Select(p => p.Id).ToList()));

            CreateMap<UpdateCategoryDto, Category>();

            // Product Mappings
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
