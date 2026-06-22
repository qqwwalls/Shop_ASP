using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private static List<Product> _products = new List<Product>();

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

        [HttpGet]
        public IActionResult GetActiveProducts()
        {
            var activeProducts = _products.Where(p => p.IsActive).ToList();
            return Ok(activeProducts);
        }

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
