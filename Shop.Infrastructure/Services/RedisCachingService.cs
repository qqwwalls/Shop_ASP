using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Shop.Application.Interfaces.Services;

namespace Shop.Infrastructure.Services;

public class RedisCachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;

    public RedisCachingService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, System.Threading.CancellationToken cancellationToken = default)
    {
        var cachedString = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(cachedString))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedString);
    }

    public async Task RemoveAsync(string key, System.Threading.CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? exp = null, System.Threading.CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = exp ?? TimeSpan.FromMinutes(15)
        };

        var jsonString = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, jsonString, options, cancellationToken);
    }
}
