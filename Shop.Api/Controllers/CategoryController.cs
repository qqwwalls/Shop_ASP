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
        private readonly StackExchange.Redis.IConnectionMultiplexer _redis;

        public CategoryController(ICategoryService categoryService, IImageService imageService, StackExchange.Redis.IConnectionMultiplexer redis)
        {
            _categoryService = categoryService;
            _imageService = imageService;
            _redis = redis;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
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

        // ================= ТЕСТОВИЙ МАРШРУТ ДЛЯ ПЕРЕВІРКИ ПОМИЛОК =================
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            // Ця помилка буде перехоплена нашим новим глобальним ErrorController
            throw new Exception("Це тестова помилка для перевірки глобального обробника!");
        }

        // ================= ЗАВДАННЯ ПО REDIS (ВІД ВИКЛАДАЧА) =================
        [HttpGet("redis-test")]
        public async Task<IActionResult> TestRedisAssignment()
        {
            var db = _redis.GetDatabase();
            var listKey = new StackExchange.Redis.RedisKey("shop_categories");
            
            // 0. Очищаємо список перед тестом, щоб не накопичувати дублікати
            await db.KeyDeleteAsync(listKey);

            var outputLogs = new System.Collections.Generic.List<string>();
            outputLogs.Add("--- ПОЧАТОК ВИКОНАННЯ ЗАВДАННЯ ---");

            // 1. Створіть список на 5 категорій товарів
            await db.ListRightPushAsync(listKey, "Електроніка");
            await db.ListRightPushAsync(listKey, "Одяг");
            await db.ListRightPushAsync(listKey, "Дім");
            await db.ListRightPushAsync(listKey, "Спорт");
            await db.ListRightPushAsync(listKey, "Книги");
            outputLogs.Add("1. Створено список з 5 категорій (RPUSH)");

            // 2. Роздрукуйте їх у консоль
            var items = await db.ListRangeAsync(listKey, 0, -1);
            outputLogs.Add("2. Друк категорій у консоль (LRANGE 0 -1):");
            foreach (var item in items)
            {
                outputLogs.Add($"   - {item}");
                Console.WriteLine($"[REDIS TEST] Категорія: {item}");
            }

            // 3. Виведіть довжину списку
            var lengthBefore = await db.ListLengthAsync(listKey);
            outputLogs.Add($"3. Довжина списку (LLEN): {lengthBefore}");
            Console.WriteLine($"[REDIS TEST] Довжина списку: {lengthBefore}");

            // 4. Видаліть третій товар у списку ("Дім")
            await db.ListRemoveAsync(listKey, "Дім", 1);
            outputLogs.Add("4. Видалено третій товар у списку - 'Дім' (LREM 1 'Дім')");
            Console.WriteLine($"[REDIS TEST] Видалено товар 'Дім'");

            // 5. Знову роздрукуйте довжину списку
            var lengthAfter = await db.ListLengthAsync(listKey);
            outputLogs.Add($"5. Знову довжина списку (LLEN): {lengthAfter}");
            Console.WriteLine($"[REDIS TEST] Нова довжина списку: {lengthAfter}");

            outputLogs.Add("--- ЗАВДАННЯ ВИКОНАНО ---");

            return Ok(new
            {
                Message = "Завдання Redis успішно виконано!",
                ConsoleOutput = outputLogs
            });
        }
    }
}
