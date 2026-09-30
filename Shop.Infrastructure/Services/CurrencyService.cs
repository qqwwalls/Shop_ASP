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
            // Використовуємо сервер викладача
            var response = await _httpClient.GetAsync("https://7c49-37-52-79-159.ngrok-free.app/currency");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(content);

            return (decimal)data.rates.UAH;
        }
    }
}
