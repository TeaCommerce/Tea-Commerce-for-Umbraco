using Autofac;
using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceStoreCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceStoreCacheRefresher, Store, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.StoreCacheRefresherGuid;

        public override string Name => "Tea Commerce Store cache refresher";

        public override string CacheKeyFormat => "Stores";

        protected override TeaCommerceStoreCacheRefresher Instance => this;
    }
}