using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awc.Services.Product.Product.API.Services
{
    public interface ICacheService
    {
        Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration);
        Task<T> GetCacheValueAsync<T>(string key);
    }
}