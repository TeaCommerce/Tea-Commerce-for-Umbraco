using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.interfaces;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public abstract class TeaCommerceCacheRefresherBase<TInstanceType, TModelType> : CacheRefresherBase<TInstanceType>
        where TInstanceType : ICacheRefresher
    {
        public int GetStoreId(TModelType model)
        {
            //Application.
            return 1;
        }
    }
}