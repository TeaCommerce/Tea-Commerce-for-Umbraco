using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCountryRegionCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCountryRegionCacheRefresher, CountryRegion, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CountryRegionCacheRefresherGuid;

        public override string Name => "Tea Commerce Country Region cache refresher";

        public override string CacheKeyFormat => "CountryRegions-{0}";

        protected override TeaCommerceCountryRegionCacheRefresher Instance => this;
    }
}