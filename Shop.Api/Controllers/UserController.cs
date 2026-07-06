using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;
using Shop.App.Filters;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        [AdminAuthFilter]
        public IActionResult Register([FromBody] User user)
        {
            return Ok(user);
        }
    }
}
