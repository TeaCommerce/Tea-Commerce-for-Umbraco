using Autofac;
using System;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Caching;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceStoreCacheRefresher : CacheRefresherBase<TeaCommerceStoreCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.StoreCacheRefresherGuid;

        public override string Name => "Tea Commerce Store cache refresher";

        protected override TeaCommerceStoreCacheRefresher Instance => this;

        protected ICacheService CacheService => DependencyContainer.Instance.Resolve<ICacheService>();

        public override void Refresh(int Id)
        {
            ClearCache();
            base.Refresh(Id);
        }

        public override void Refresh(Guid Id)
        {
            ClearCache();
            base.Refresh(Id);
        }

        public override void RefreshAll()
        {
            ClearCache();
            base.RefreshAll();
        }

        public override void Remove(int Id)
        {
            ClearCache();
            base.Remove(Id);
        }

        protected void ClearCache()
        {
            CacheService.Invalidate("Stores");
        }
    }
}