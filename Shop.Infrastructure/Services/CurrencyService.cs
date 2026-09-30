using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shop.Application.Interfaces.Services;

namespace Shop.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetUsdRateAsync()
        {
            // Используем тестовый URL, который вернет 500 ошибку, чтобы Polly срабатывал и делал ретраи
            // Но в идеале здесь должен быть реальный API.
            var response = await _httpClient.GetAsync("https://httpstat.us/500");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(content);

            return (decimal)data.rates.UAH;
        }
    }
}
