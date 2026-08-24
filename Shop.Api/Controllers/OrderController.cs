using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize] // Тільки для авторизованих користувачів
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Дістаємо ID користувача безпосередньо з його JWT токена
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid token or user ID missing." });
            }

            // Створюємо анонімний об'єкт, який поєднує UserId з токена та дані з фронтенду
            var orderMessage = new 
            {
                UserId = userId,
                Status = orderDto.Status,
                Paid = orderDto.Paid,
                Items = orderDto.Items
            };

            await _queueService.PublishAsync("Orders", orderMessage);

            return Ok(new { message = "Order successfully sent to processing queue." });
        }
    }
}
