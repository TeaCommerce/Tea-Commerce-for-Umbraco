using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCurrencyCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceCurrencyCacheRefresher, Currency, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.CurrencyCacheRefresherGuid;

        public override string Name => "Tea Commerce Currency cache refresher";

        public override string CacheKeyFormat => "Currencies-{0}";

        public override Func<Currency, long> IdAccessor => x => x.Id;

        protected override TeaCommerceCurrencyCacheRefresher Instance => this;
    }
}