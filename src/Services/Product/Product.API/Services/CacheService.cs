#pragma warning disable CS8613

using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Awc.Services.Product.Product.API.Services
{
    public sealed class CacheService(IConnectionMultiplexer redis) : ICacheService
    {
        private readonly IConnectionMultiplexer _redis = redis;

        public async Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize(value);
            await db.StringSetAsync(key, json, expiration);
        }

        public async Task<T?> GetCacheValueAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync(key);
            return json.HasValue ? JsonSerializer.Deserialize<T>(json!) : default;
        }
    }
}