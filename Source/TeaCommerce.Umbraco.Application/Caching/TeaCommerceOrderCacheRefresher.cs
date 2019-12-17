using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceOrderCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceOrderCacheRefresher, Order, Guid>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.OrderCacheRefresherGuid;

        public override string Name => "Tea Commerce Order cache refresher";

        public override string CacheKeyFormat => "Orders-{0}";

        public override Func<Order, Guid> IdAccessor => x => x.Id;

        protected override TeaCommerceOrderCacheRefresher Instance => this;
    }
}