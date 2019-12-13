using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using TeaCommerce.Api.Notifications;
using TeaCommerce.Umbraco.Application.Caching;
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
            NotificationCenter.Campaign.Created += (e) => { InvalidateDistributedCache(Constants.DistributedCache.CampaignCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.Campaign.Updated += (e) => { InvalidateDistributedCache(Constants.DistributedCache.CampaignCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.Campaign.Deleted += (e) => { InvalidateDistributedCache(Constants.DistributedCache.CampaignCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.Country.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.Country.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.Country.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.CountryRegion.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryRegionCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.CountryRegion.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryRegionCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.CountryRegion.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CountryRegionCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.Currency.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CurrencyCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.Currency.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CurrencyCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.Currency.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.CurrencyCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.EmailTemplate.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.EmailTemplateCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.EmailTemplate.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.EmailTemplateCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.EmailTemplate.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.EmailTemplateCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.Order.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.Order.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.Order.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.OrderStatus.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderStatusCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.OrderStatus.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderStatusCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.OrderStatus.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.OrderStatusCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.PaymentMethod.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.PaymentMethodCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.PaymentMethod.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.PaymentMethodCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.PaymentMethod.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.PaymentMethodCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.ShippingMethod.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.ShippingMethodCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.ShippingMethod.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.ShippingMethodCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.ShippingMethod.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.ShippingMethodCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.Store.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.StoreCacheRefresherGuid, e.Id, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.Store.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.StoreCacheRefresherGuid, e.Id, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.Store.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.StoreCacheRefresherGuid, e.Id, e.Id, CacheRefresherAction.Deleted); };

            NotificationCenter.VatGroup.Created += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.VatGroupCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Created); };
            NotificationCenter.VatGroup.Updated += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.VatGroupCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Updated); };
            NotificationCenter.VatGroup.Deleted += (e, a) => { InvalidateDistributedCache(Constants.DistributedCache.VatGroupCacheRefresherGuid, e.StoreId, e.Id, CacheRefresherAction.Deleted); };
        }

        private void InvalidateDistributedCache<TId>(Guid cacheKey, long storeId, TId id, CacheRefresherAction action)
        {
            var payload = new TeaCommerceCacheRefresherPayload<TId>
            {
                InstanceId = Constants.InstanceId,
                StoreId = storeId,
                Id = id,
                Action = action
            };

            DistributedCache.Instance.RefreshByJson(cacheKey, JsonConvert.SerializeObject(payload));
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