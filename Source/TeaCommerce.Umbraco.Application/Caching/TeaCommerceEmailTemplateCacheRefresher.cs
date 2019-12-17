using System;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceEmailTemplateCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommerceEmailTemplateCacheRefresher, EmailTemplate, long>
    {
        public override Guid UniqueIdentifier => Constants.DistributedCache.EmailTemplateCacheRefresherGuid;

        public override string Name => "Tea Commerce Email Template cache refresher";

        public override string CacheKeyFormat => "EmailTemplates-{0}";

        public override Func<EmailTemplate, long> IdAccessor => x => x.Id;

        protected override TeaCommerceEmailTemplateCacheRefresher Instance => this;
    }
}