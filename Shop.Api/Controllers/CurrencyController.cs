using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.Services;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("usd")]
        public async Task<IActionResult> GetUsd()
        {
            // Цей метод викличе ICurrencyService, який зробить запит через HttpClient,
            // і завдяки неправильному URL, Polly перехопить помилку і виконає retry / circuit breaker!
            var rate = await _currencyService.GetUsdRateAsync();
            return Ok(rate);
        }
    }
}
