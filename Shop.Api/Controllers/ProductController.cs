using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Interfaces.Services;
using Shop.Application.Queries.GetProductById;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMediator _mediator;

        public ProductController(IProductService productService, IMediator mediator)
        {
            _productService = productService;
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] CreateProductDto dto)
        {
            var product = _productService.CreateProduct(dto);
            return Ok(product);
        }

        [HttpGet]
        public IActionResult GetActiveProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            // Використання патерну CQRS через MediatR
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _mediator.Send(new Shop.Application.Commands.DeleteProduct.DeleteProductCommand(id));
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var product = _productService.UpdateProduct(id, dto);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
