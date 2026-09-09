using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetItems(CancellationToken cancellationToken)
        {
            var dummyItems = new List<string> { "Item1", "Item2", "Item3" };
            return Ok(dummyItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetItem(int id, CancellationToken cancellationToken)
        {
            var dummyItem = $"Item{id}";
            return Ok(new { Id = id, Name = dummyItem });
        }
    }
}
