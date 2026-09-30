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

            // Використовуємо строго типізовану модель замість dynamic
            var data = JsonConvert.DeserializeObject<System.Collections.Generic.List<Shop.Application.Models.CurrencyRateModel>>(content);

            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item.cc == "USD")
                    {
                        return item.rate;
                    }
                }
            }

            return 0m;
        }
    }
}
