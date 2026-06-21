using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private static List<Product> _products = new List<Product>();

        // Метод POST додає продукт у список, якщо продукта з таким самим ім'ям не має
        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (_products.Any(p => p.Title == product.Title))
            {
                return BadRequest("Product with the same title already exists.");
            }
            _products.Add(product);
            return Ok(product);
        }

        // Метод GET видає всі продукти, які IsActive == true
        [HttpGet]
        public IActionResult GetActiveProducts()
        {
            var activeProducts = _products.Where(p => p.IsActive).ToList();
            return Ok(activeProducts);
        }

        // Метод GET /id видає 1 продукт з масива, якщо немає NotFound (404)
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Метод DELETE видаляє продукта за id, якщо його немає NotFound (404)
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _products.Remove(product);
            return Ok();
        }

        // Метод PUT дозволяє змінити дані про продукт за певним id, якщо його немає NotFound (404)
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Title = updatedProduct.Title;
            product.Price = updatedProduct.Price;
            product.Count = updatedProduct.Count;
            product.Discount = updatedProduct.Discount;
            product.IsActive = updatedProduct.IsActive;

            return Ok(product);
        }
    }
}
