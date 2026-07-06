using System;
using System.Collections.Generic;
using AutoMapper;
using Shop.Domain.Models;
using Shop.Application.Interfaces.Services;
using Shop.Application.Interfaces.Repository;
using Shop.Application.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Services;

public class CategoryService(ICategoryRepository _repository, IMapper _mapper) : ICategoryService
{
    public List<CategoryReadDTO> GetAllCategories()
    {
        var categories = _repository.GetAllCategories().ToList();
        return _mapper.Map<List<CategoryReadDTO>>(categories);
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var category = await _repository.GetCategoryByIdAsync(id);
        if (category == null) return null;
        return _mapper.Map<CategoryReadDTO>(category);
    }

    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        return await _repository.AddCategoryAsync(new Category()
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Url = dto.Url,
            ParentId = dto.ParentId,
        });
    }
}
