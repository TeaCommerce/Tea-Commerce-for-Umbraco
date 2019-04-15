using System;
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
            SetupDistributedCacheInvalidation();
            SetupContentCopyingHandler();
        }

        private void SetupDistributedCacheInvalidation()
        {
            NotificationCenter.Campaign.Created += (e) => { InvalidateDistributedCache(Constants.DistributedCache.CampaignCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Campaign.Updated += (e) => { InvalidateDistributedCache(Constants.DistributedCache.CampaignCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Campaign.Deleted += (e) => { InvalidateDistributedCache(Constants.DistributedCache.CampaignCacheRefresherGuid, e.StoreId); };

            NotificationCenter.Country.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Country.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Country.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryCacheRefresherGuid, e.StoreId); };

            NotificationCenter.CountryRegion.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryRegionCacheRefresherGuid, e.StoreId); };
            NotificationCenter.CountryRegion.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryRegionCacheRefresherGuid, e.StoreId); };
            NotificationCenter.CountryRegion.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryRegionCacheRefresherGuid, e.StoreId); };

            NotificationCenter.Currency.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CurrencyCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Currency.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CurrencyCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Currency.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CurrencyCacheRefresherGuid, e.StoreId); };

            NotificationCenter.EmailTemplate.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.EmailTemplateCacheRefresherGuid, e.StoreId); };
            NotificationCenter.EmailTemplate.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.EmailTemplateCacheRefresherGuid, e.StoreId); };
            NotificationCenter.EmailTemplate.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.EmailTemplateCacheRefresherGuid, e.StoreId); };

            NotificationCenter.Order.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Order.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderCacheRefresherGuid, e.StoreId); };
            NotificationCenter.Order.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderCacheRefresherGuid, e.StoreId); };

            NotificationCenter.OrderStatus.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderStatusCacheRefresherGuid, e.StoreId); };
            NotificationCenter.OrderStatus.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderStatusCacheRefresherGuid, e.StoreId); };
            NotificationCenter.OrderStatus.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderStatusCacheRefresherGuid, e.StoreId); };

            NotificationCenter.PaymentMethod.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.PaymentMethodCacheRefresherGuid, e.StoreId); };
            NotificationCenter.PaymentMethod.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.PaymentMethodCacheRefresherGuid, e.StoreId); };
            NotificationCenter.PaymentMethod.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.PaymentMethodCacheRefresherGuid, e.StoreId); };

            NotificationCenter.ShippingMethod.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.ShippingMethodCacheRefresherGuid, e.StoreId); };
            NotificationCenter.ShippingMethod.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.ShippingMethodCacheRefresherGuid, e.StoreId); };
            NotificationCenter.ShippingMethod.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.ShippingMethodCacheRefresherGuid, e.StoreId); };

            NotificationCenter.Store.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.StoreCacheRefresherGuid); };
            NotificationCenter.Store.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.StoreCacheRefresherGuid); };
            NotificationCenter.Store.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.StoreCacheRefresherGuid); };

            NotificationCenter.VatGroup.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.VatGroupCacheRefresherGuid, e.StoreId); };
            NotificationCenter.VatGroup.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.VatGroupCacheRefresherGuid, e.StoreId); };
            NotificationCenter.VatGroup.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.VatGroupCacheRefresherGuid, e.StoreId); };
        }

        private void InvalidateDistributedCache(Guid cacheKey, long? storeId = null)
        {
            if (storeId.HasValue)
            {
                DistributedCache.Instance.Refresh(cacheKey, (int)storeId);
            }
            else
            {
                DistributedCache.Instance.RefreshAll(cacheKey);
            }
        }

        private void SetupContentCopyingHandler()
        {
            ContentService.Copying += ContentService_Copying;
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