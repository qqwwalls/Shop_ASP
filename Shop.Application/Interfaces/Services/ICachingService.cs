using System;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces.Services;

public interface ICachingService
{
    Task<T?> GetAsync<T>(string key, System.Threading.CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan? exp = null, System.Threading.CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, System.Threading.CancellationToken cancellationToken = default);
}
