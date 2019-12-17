using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Caching;
using umbraco.interfaces;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public abstract class TeaCommerceCacheRefresherBase<TInstanceType, TEntity, TId> : JsonCacheRefresherBase<TInstanceType>
        where TInstanceType : ICacheRefresher
    {
        protected ICacheService CacheService => DependencyContainer.Instance.Resolve<ICacheService>();

        public abstract string CacheKeyFormat { get; }

        public override void Refresh(string jsonPayload)
        {
            var payload = JsonConvert.DeserializeObject<TeaCommerceCacheRefresherPayload<TId>>(jsonPayload);

            // Make sure it wasn't this instance that sent the payload
            if (payload.InstanceId != Constants.InstanceId)
            {
                var cacheKey = string.Format(CacheKeyFormat, payload.StoreId, payload.Id);
                var cache = CacheService.GetCacheValue<ConcurrentDictionary<TId, TEntity>>(cacheKey);
                if (cache.ContainsKey(payload.Id))
                {
                    cache.TryRemove(payload.Id, out var removed);
                }

                base.Refresh(jsonPayload);
            }
        }

        public override void Refresh(int id)
        {
            throw new NotSupportedException();
        }

        public override void Refresh(Guid id)
        {
            throw new NotSupportedException();
        }

        public override void RefreshAll()
        {
            throw new NotSupportedException();
        }

        public override void Remove(int id)
        {
            throw new NotSupportedException();
        }
    }
}