using System;
using System.Collections.Generic;
using ShopDomain.Models;
using ShopApp.Interfaces;

namespace ShopApp.Services
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
                    Title = "Electronics", 
                    Description = "Gadgets and devices", 
                    Image = "electronics.png", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                    IsShow = true 
                },
                new Category 
                { 
                    Id = 2, 
                    Title = "Books", 
                    Description = "Printed materials", 
                    Image = "books.png", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                    IsShow = true 
                }
            };
        }

        public List<Category> GetAllCategories()
        {
            return _categories;
        }
    }
}
