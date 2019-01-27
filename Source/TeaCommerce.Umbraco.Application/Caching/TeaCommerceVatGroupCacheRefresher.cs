using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceVatGroupCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceVatGroupCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.VatGroupCacheRefresherGuid;

        public override string Name => "Tea Commerce VAT Group cache refresher";

        protected override TeaCommerceVatGroupCacheRefresher Instance => this;

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
            CacheService.Invalidate($"VatGroups-{storeId}");
        }
    }
}