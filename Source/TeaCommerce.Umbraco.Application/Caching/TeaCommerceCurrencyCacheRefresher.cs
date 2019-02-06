using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCurrencyCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCurrencyCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CurrencyCacheRefresherGuid;

        public override string Name => "Tea Commerce Currency cache refresher";

        protected override TeaCommerceCurrencyCacheRefresher Instance => this;

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
            CacheService.Invalidate($"Currencies-{storeId}");
        }
    }
}