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

    public async Task<T?> GetAsync<T>(string key)
    {
        var cachedString = await _distributedCache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cachedString))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedString);
    }

    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? exp = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = exp ?? TimeSpan.FromMinutes(15)
        };

        var jsonString = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, jsonString, options);
    }
}
