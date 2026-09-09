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

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICachingService _cachingService;

    public CategoryService(ICategoryRepository repository, IMapper mapper, ICachingService cachingService)
    {
        _repository = repository;
        _mapper = mapper;
        _cachingService = cachingService;
    }

    public async Task<List<CategoryReadDTO>> GetAllCategoriesAsync(System.Threading.CancellationToken cancellationToken = default)
    {
        var cacheKey = "Categories";
        var cachedCategories = await _cachingService.GetAsync<List<CategoryReadDTO>>(cacheKey, cancellationToken);

        if (cachedCategories != null)
        {
            return cachedCategories;
        }

        var categories = _repository.GetAllCategories().ToList();
        var dtos = _mapper.Map<List<CategoryReadDTO>>(categories);

        await _cachingService.SetAsync(cacheKey, dtos, cancellationToken: cancellationToken);

        return dtos;
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id, System.Threading.CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Category_{id}";
        var cachedCategory = await _cachingService.GetAsync<CategoryReadDTO>(cacheKey, cancellationToken);
        
        if (cachedCategory != null)
        {
            return cachedCategory;
        }

        var category = await _repository.GetCategoryByIdAsync(id, cancellationToken);
        if (category == null) return null;
        
        var dto = _mapper.Map<CategoryReadDTO>(category);
        await _cachingService.SetAsync(cacheKey, dto, cancellationToken: cancellationToken);
        
        return dto;
    }

    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto, System.Threading.CancellationToken cancellationToken = default)
    {
        var category = _mapper.Map<Category>(dto);
        var result = await _repository.AddCategoryAsync(category, cancellationToken);

        await _cachingService.RemoveAsync("Categories", cancellationToken);

        return result;
    }
}
