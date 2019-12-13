using System;
using TeaCommerce.Api.Marketing.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCampaignCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCampaignCacheRefresher, Campaign, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CampaignCacheRefresherGuid;

        public override string Name => "Tea Commerce Campaign cache refresher";

        public override string CacheKeyFormat => "Campaigns-{0}";

        public override Func<Campaign, long> IdAccessor => x => x.Id;

        protected override TeaCommerceCampaignCacheRefresher Instance => this;
    }
}