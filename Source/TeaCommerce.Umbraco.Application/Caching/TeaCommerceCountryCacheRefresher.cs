using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCountryCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCountryCacheRefresher, Country, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CountryCacheRefresherGuid;

        public override string Name => "Tea Commerce Country cache refresher";

        public override string CacheKeyFormat => "Countries-{0}";

        public override Func<Country, long> IdAccessor => x => x.Id;

        protected override TeaCommerceCountryCacheRefresher Instance => this;
    }
}