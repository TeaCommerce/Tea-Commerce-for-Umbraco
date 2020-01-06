using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceShippingMethodCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceShippingMethodCacheRefresher, ShippingMethod, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.ShippingMethodCacheRefresherGuid;

        public override string Name => "Tea Commerce Shipping Method cache refresher";

        public override string CacheKeyFormat => "ShippingMethods-{0}";

        protected override TeaCommerceShippingMethodCacheRefresher Instance => this;
    }
}