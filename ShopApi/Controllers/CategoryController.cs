using Microsoft.AspNetCore.Mvc;
using ShopApplication.Interfaces;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }
    }
}
