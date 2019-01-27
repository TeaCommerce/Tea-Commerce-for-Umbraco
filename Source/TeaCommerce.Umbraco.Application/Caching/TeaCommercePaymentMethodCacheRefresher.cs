﻿using System;
using Umbraco.Core.Cache;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommercePaymentMethodCacheRefresher : CacheRefresherBase<TeaCommercePaymentMethodCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.PaymentMethodCacheRefresherGuid;

        public override string Name => "Tea Commerce Payment Method cache refresher";

        protected override TeaCommercePaymentMethodCacheRefresher Instance
        {
            get { return this; }
        }

        public override void Refresh(int Id)
        {
            ClearCache(Id);
            base.Refresh(Id);
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
            ClearCache(Id);
            base.Remove(Id);
        }

        protected void ClearCache(int? Id = null)
        {
            if (Id.HasValue)
            {
                // TODO: Get storeId from entity ID
                // TODO: Clear store specific cache
            }
            else
            {
                // TODO: Get all storeIds
                // TODO: Clear all store caches
            }
        }
    }
}