using System;
using System.Collections.Generic;
using ShopDomain.Models;
using ShopApplication.Interfaces;

namespace ShopApplication.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly List<Category> _categories;

        public CategoryService()
        {
            _categories = new List<Category>
            {
                new Category 
                { 
                    Id = 1, 
                    Name = "Electronics", 
                    Slug = "electronics", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                },
                new Category 
                { 
                    Id = 2, 
                    Name = "Books", 
                    Slug = "books", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                }
            };
        }

        public List<Category> GetAllCategories()
        {
            return _categories;
        }
    }
}
