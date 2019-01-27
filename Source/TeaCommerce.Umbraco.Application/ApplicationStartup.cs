using System.Globalization;
using System.Linq;
using TeaCommerce.Api.Notifications;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Cache;

namespace TeaCommerce.Umbraco.Application
{

    public class ApplicationStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            NotificationCenter.Store.Created += (s, e) => { InvalidateStoresCache(); };
            NotificationCenter.Store.Deleted += (s, e) => { InvalidateStoresCache(); };

            NotificationCenter.Order.Created += (o, e) => { InvalidateOrdersCache(o.StoreId); };
            NotificationCenter.Order.Updated += (o, e) => { InvalidateOrdersCache(o.StoreId); };
            NotificationCenter.Order.Deleted += (o, e) => { InvalidateOrdersCache(o.StoreId); };

            ContentService.Copying += ContentService_Copying;
        }

        private void InvalidateStoresCache()
        {
            DistributedCache.Instance.RefreshAll(Constants.DistributedCache.StoreCacheRefresherGuid);
        }

        private void InvalidateOrdersCache(long storeId)
        {
            DistributedCache.Instance.Refresh(Constants.DistributedCache.OrderCacheRefresherGuid, (int)storeId);
        }

        private void ContentService_Copying(IContentService sender, CopyEventArgs<IContent> e)
        {
            Property masterRelationProperty = e.Copy.Properties.SingleOrDefault(p => p.Alias == Api.Constants.ProductPropertyAliases.MasterRelationPropertyAlias);

            if (masterRelationProperty == null || (masterRelationProperty.Value != null && !string.IsNullOrEmpty(masterRelationProperty.Value.ToString())))
            {
                return;
            }

            // Delete all property data
            foreach (Property property in e.Copy.Properties)
            {
                property.Value = null;
            }

            masterRelationProperty.Value = e.Original.Id.ToString(CultureInfo.InvariantCulture);
        }

    }
}