using System;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceEmailTemplateCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceEmailTemplateCacheRefresher>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.EmailTemplateCacheRefresherGuid;

        public override string Name => "Tea Commerce Email Template cache refresher";

        protected override TeaCommerceEmailTemplateCacheRefresher Instance => this;

        public override void Refresh(int Id)
        {
            // Id = storeId
            ClearCache(Id);
            base.Refresh(Id);
        }

        public override void RefreshAll()
        {
            throw new NotImplementedException();
        }

        public override void Remove(int Id)
        {
            // Id = storeId
            ClearCache(Id);
            base.Remove(Id);
        }

        protected void ClearCache(int storeId)
        {
            CacheService.Invalidate($"EmailTemplates-{storeId}");
        }
    }
}