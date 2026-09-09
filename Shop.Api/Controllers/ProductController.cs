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
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto dto, System.Threading.CancellationToken cancellationToken)
        {
            var product = await _productService.CreateProductAsync(dto, cancellationToken);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveProducts(System.Threading.CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProductsAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id, System.Threading.CancellationToken cancellationToken)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, System.Threading.CancellationToken cancellationToken)
        {
            var success = await _mediator.Send(new Shop.Application.Commands.DeleteProduct.DeleteProductCommand(id), cancellationToken);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto, System.Threading.CancellationToken cancellationToken)
        {
            var product = await _productService.UpdateProductAsync(id, dto, cancellationToken);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
