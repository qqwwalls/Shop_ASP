using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.Services;
using Shop.Application.DTOs;
using System.Threading.Tasks;
using Shop.Api.Interfaces;
using Shop.Api.Requests.Categories;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;

        public CategoryController(ICategoryService categoryService, IImageService imageService)
        {
            _categoryService = categoryService;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDTO>> GetCategoryById(int id)
        {
            var dto = await _categoryService.GetCategoryByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest request)
        {
            var imageUrl = string.Empty;
            if (request.Image != null)
            {
                imageUrl = (await _imageService.SaveFileAsync(request.Image, "categories")) ?? string.Empty;
            }

            var createDto = new CategoryCreateDTO
            {
                Name = request.Name,
                Url = imageUrl,
                Slug = request.Slug,
                ParentId = request.ParentId,
            };

            var id = await _categoryService.CreateCategoryAsync(createDto);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id },
                new { id });
        }
    }
}
