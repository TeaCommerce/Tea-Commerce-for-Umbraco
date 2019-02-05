using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCountryRegionCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCountryRegionCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CountryRegionCacheRefresherGuid;

        public override string Name => "Tea Commerce Country Region cache refresher";

        protected override TeaCommerceCountryRegionCacheRefresher Instance => this;

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
            CacheService.Invalidate($"CountryRegions-{storeId}");
        }
    }
}