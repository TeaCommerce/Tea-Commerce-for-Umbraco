using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceShippingMethodCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceShippingMethodCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.ShippingMethodCacheRefresherGuid;

        public override string Name => "Tea Commerce Shipping Method cache refresher";

        protected override TeaCommerceShippingMethodCacheRefresher Instance => this;

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
            CacheService.Invalidate($"ShippingMethods-{storeId}");
        }
    }
}