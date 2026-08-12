using System;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces.Services;

public interface ICachingService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? exp = null);
    Task RemoveAsync(string key);
}
