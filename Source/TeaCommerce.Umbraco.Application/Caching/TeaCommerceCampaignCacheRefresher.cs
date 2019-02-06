using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCampaignCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCampaignCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CampaignCacheRefresherGuid;

        public override string Name => "Tea Commerce Campaign cache refresher";

        protected override TeaCommerceCampaignCacheRefresher Instance => this;

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
            CacheService.Invalidate($"Campaigns-{storeId}");
        }
    }
}