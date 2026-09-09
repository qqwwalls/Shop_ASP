using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public OrderController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _queueService.PublishAsync("Orders", orderDto, cancellationToken);

            return Ok(new { message = "Order sent to RabbitMQ (for MongoDB processing)." });
        }
    }
}
