using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Caching;
using umbraco.interfaces;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public abstract class TeaCommerceCacheRefresherBase<TInstanceType, TEntity, TId> : JsonCacheRefresherBase<TInstanceType>
        where TInstanceType : ICacheRefresher
    {
        private readonly SyncLock<long> _sync = new SyncLock<long>();

        protected ICacheService CacheService => DependencyContainer.Instance.Resolve<ICacheService>();

        public abstract string CacheKeyFormat { get; }

        public abstract Func<TEntity, TId> IdAccessor { get; }

        public override void Refresh(string jsonPayload)
        {
            var payload = JsonConvert.DeserializeObject<TeaCommerceCacheRefresherPayload<TId>>(jsonPayload);

            // Make sure it wasn't this instance that sent the payload
            if (payload.InstanceId != Constants.InstanceId)
            {
                using (_sync.Lock(payload.StoreId))
                {
                    var cacheKey = string.Format(CacheKeyFormat, payload.StoreId, payload.Id);
                    var cache = CacheService.GetCacheValue<List<TEntity>>(cacheKey);
                    var entity = cache.FirstOrDefault(x => IdAccessor(x).Equals(payload.Id));
                    if (entity != null)
                    {
                        cache.Remove(entity);
                    }
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