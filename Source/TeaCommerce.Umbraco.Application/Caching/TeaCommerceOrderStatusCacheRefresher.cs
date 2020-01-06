using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceOrderStatusCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceOrderStatusCacheRefresher, OrderStatus, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.OrderStatusCacheRefresherGuid;

        public override string Name => "Tea Commerce Order Status cache refresher";

        public override string CacheKeyFormat => "OrderStatuses-{0}";

        protected override TeaCommerceOrderStatusCacheRefresher Instance => this;
    }
}