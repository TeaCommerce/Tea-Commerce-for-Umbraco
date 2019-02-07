using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceOrderStatusCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceOrderStatusCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.OrderStatusCacheRefresherGuid;

        public override string Name => "Tea Commerce Order Status cache refresher";

        protected override TeaCommerceOrderStatusCacheRefresher Instance => this;

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
            CacheService.Invalidate($"OrderStatuses-{storeId}");
        }
    }
}