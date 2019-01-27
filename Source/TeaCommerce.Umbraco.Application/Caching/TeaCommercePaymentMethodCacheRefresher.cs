using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommercePaymentMethodCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommercePaymentMethodCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.PaymentMethodCacheRefresherGuid;

        public override string Name => "Tea Commerce Payment Method cache refresher";

        protected override TeaCommercePaymentMethodCacheRefresher Instance => this;

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
            CacheService.Invalidate($"PaymentMethods-{storeId}");
        }
    }
}