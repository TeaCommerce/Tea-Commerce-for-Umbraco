using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceVatGroupCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceVatGroupCacheRefresher, VatGroup, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.VatGroupCacheRefresherGuid;

        public override string Name => "Tea Commerce VAT Group cache refresher";

        public override string CacheKeyFormat => "VatGroups-{0}";

        protected override TeaCommerceVatGroupCacheRefresher Instance => this;
    }
}