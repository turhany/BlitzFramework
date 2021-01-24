using System;
using System.Threading;
using BlitzFramework.Cache.Abstract;
using BlitzFramework.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
// ReSharper disable SimplifyConditionalTernaryExpression

namespace BlitzFramework.Cache.Concrete
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public T GetOrSetObject<T>(string key, Func<T> code, int durationAsMinute = FrameworkConstants.DefaultCacheDuration)
        {
            if (ExistObject<T>(key))
            {
                return GetObject<T>(key);
            }

            var result = code.Invoke();

            SetObject(key, result, durationAsMinute);
            return result;
        }

        public void SetObject<T>(string key, T value, int durationAsMinute = FrameworkConstants.DefaultCacheDuration)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            _distributedCache.SetString(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(durationAsMinute)
            });
        }

        public T GetObject<T>(string key)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            var value = _distributedCache.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public bool ExistObject<T>(string key)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            var value = _distributedCache.GetString(key);
            return value == null ? false : true;
        }
    }
}