using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;
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
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Відправляємо замовлення у RabbitMQ для подальшого збереження в MongoDB
            await _queueService.PublishAsync("Orders", orderDto);

            return Ok(new { message = "Order sent to RabbitMQ (for MongoDB processing)." });
        }
    }
}
