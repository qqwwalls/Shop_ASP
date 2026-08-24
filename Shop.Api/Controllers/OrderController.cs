using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;
using System.Threading.Tasks;

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

            await _queueService.PublishAsync("Orders", orderDto);

            return Ok(new { message = "Order successfully sent to processing queue." });
        }
    }
}
