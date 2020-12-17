using System;
using BlitzFramework.Constants;

namespace BlitzFramework.Cache.Abstract
{
    public interface ICacheService
    {
        T GetOrSetObject<T>(string key, Func<T> code, int durationAsMinute = AppConstants.DefaultCacheDuration);
        void SetObject<T>(string key, T value, int durationAsMinute = AppConstants.DefaultCacheDuration);
        T GetObject<T>(string key);
        bool ExistObject<T>(string key);
    }
}