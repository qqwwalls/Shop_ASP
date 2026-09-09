using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("without")]
        public async Task<IActionResult> TestWithoutCT(CancellationToken token)
        {
            _logger.LogInformation("start test withoutCT");
            await Task.Delay(10000); 
            _logger.LogInformation("action 1");
            await Task.Delay(2000);
            _logger.LogInformation("action 2");
            await Task.Delay(500);

            _logger.LogInformation("end test withoutCT");

            return Ok("End");
        }

        [HttpGet("with")]
        public async Task<IActionResult> TestWithCT(CancellationToken token)
        {
            _logger.LogInformation("start test withCT"); 
            await Task.Delay(10000, token); 
            _logger.LogInformation("action 1");
            await Task.Delay(2000, token);
            _logger.LogInformation("action 2");
            await Task.Delay(500, token);

            _logger.LogInformation("end test withCT");

            return Ok("End");
        }
    }
}
