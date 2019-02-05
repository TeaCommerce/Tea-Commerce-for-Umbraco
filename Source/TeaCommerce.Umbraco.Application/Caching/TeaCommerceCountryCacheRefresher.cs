using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCountryCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCountryCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CountryCacheRefresherGuid;

        public override string Name => "Tea Commerce Country cache refresher";

        protected override TeaCommerceCountryCacheRefresher Instance => this;

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
            CacheService.Invalidate($"Countries-{storeId}");
        }
    }
}