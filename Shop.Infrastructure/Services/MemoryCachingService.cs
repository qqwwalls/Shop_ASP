using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Shop.Application.Interfaces.Services;

namespace Shop.Infrastructure.Services;

public class MemoryCachingService : ICachingService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCachingService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<T?> GetAsync<T>(string key, System.Threading.CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(key, out T value))
        {
            return Task.FromResult<T?>(value);
        }

        return Task.FromResult<T?>(default);
    }

    public Task RemoveAsync(string key, System.Threading.CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? exp = null, System.Threading.CancellationToken cancellationToken = default)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = exp ?? TimeSpan.FromMinutes(15)
        };

        _memoryCache.Set(key, value, options);
        return Task.CompletedTask;
    }
}
