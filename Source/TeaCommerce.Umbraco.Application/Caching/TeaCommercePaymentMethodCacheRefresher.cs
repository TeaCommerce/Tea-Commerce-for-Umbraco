using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommercePaymentMethodCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommercePaymentMethodCacheRefresher, PaymentMethod, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.PaymentMethodCacheRefresherGuid;

        public override string Name => "Tea Commerce Payment Method cache refresher";

        public override string CacheKeyFormat => "PaymentMethods-{0}";

        protected override TeaCommercePaymentMethodCacheRefresher Instance => this;
    }
}