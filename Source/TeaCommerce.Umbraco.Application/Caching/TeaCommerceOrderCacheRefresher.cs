using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceOrderCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceOrderCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.OrderCacheRefresherGuid;

        public override string Name => "Tea Commerce Order cache refresher";

        protected override TeaCommerceOrderCacheRefresher Instance => this;

        public override void Refresh(int Id)
        {
            // Id = storeId
            ClearCache(Id);
            base.Refresh(Id);
        }

        public override void RefreshAll()
        {
            throw new NotImplementedException();
        }

        public override void Remove(int Id)
        {
            // Id = storeId
            ClearCache(Id);
            base.Remove(Id);
        }

        protected void ClearCache(int storeId)
        {
            CacheService.Invalidate($"Orders-{storeId}");
        }
    }
}