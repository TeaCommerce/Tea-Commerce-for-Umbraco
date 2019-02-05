using Autofac;
using System;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Caching;
using umbraco.interfaces;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public abstract class TeaCommerceCacheRefresherBase<TInstanceType> : CacheRefresherBase<TInstanceType>
        where TInstanceType : ICacheRefresher
    {
        protected ICacheService CacheService => DependencyContainer.Instance.Resolve<ICacheService>();
        
        public override void Refresh(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}