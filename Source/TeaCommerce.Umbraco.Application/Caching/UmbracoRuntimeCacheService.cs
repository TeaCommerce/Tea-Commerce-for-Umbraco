using System;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Caching;
using Umbraco.Core;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    [SuppressDependency("TeaCommerce.Api.Infrastructure.Caching.ICacheService", "TeaCommerce.Api")]
    public class UmbracoRuntimeCacheService : ICacheService
    {
        private IRuntimeCacheProvider _runtimeCache;

        public UmbracoRuntimeCacheService()
            : this(ApplicationContext.Current.ApplicationCache.RuntimeCache)
        { }

        public UmbracoRuntimeCacheService(IRuntimeCacheProvider runtimeCache)
        {
            _runtimeCache = runtimeCache;
        }

        public T GetCacheValue<T>(string cacheKey) where T : class
        {
            return (T)_runtimeCache.GetCacheItem($"TeaCommerce_{cacheKey}");
        }

        public void Invalidate(string cacheKey)
        {
            _runtimeCache.ClearCacheItem($"TeaCommerce_{cacheKey}");
        }

        public void SetCacheValue(string cacheKey, object cacheValue)
        {
            _runtimeCache.InsertCacheItem($"TeaCommerce_{cacheKey}", () => cacheValue);
        }

        public void SetCacheValue(string cacheKey, object cacheValue, TimeSpan cacheDuration)
        {
            _runtimeCache.InsertCacheItem($"TeaCommerce_{cacheKey}", () => cacheValue, cacheDuration, true);
        }
    }
}