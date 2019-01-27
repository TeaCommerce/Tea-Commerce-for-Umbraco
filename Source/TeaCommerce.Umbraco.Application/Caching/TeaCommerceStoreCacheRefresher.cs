using Autofac;
using System;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Caching;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceStoreCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceStoreCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.StoreCacheRefresherGuid;

        public override string Name => "Tea Commerce Store cache refresher";

        protected override TeaCommerceStoreCacheRefresher Instance => this;

        public override void Refresh(int Id)
        {
            throw new NotImplementedException();
        }

        public override void Refresh(Guid Id)
        {
            throw new NotImplementedException();
        }

        public override void RefreshAll()
        {
            ClearCache();
            base.RefreshAll();
        }

        public override void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        protected void ClearCache()
        {
            CacheService.Invalidate("Stores");
        }
    }
}