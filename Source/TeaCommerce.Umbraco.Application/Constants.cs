using System;

namespace TeaCommerce.Umbraco.Application
{
    public class Constants
    {
        public static string InstanceId = Guid.NewGuid().ToString("N").ToUpper();

        public class Applications
        {
            public const string TeaCommerce = "teacommerce";
        }

        public class EditorIcons
        {
            public const string Edit = "TeaCommerce.Umbraco.Application.Content.Images.EditorIcons.edit.gif";
            public const string Delete = "TeaCommerce.Umbraco.Application.Content.Images.EditorIcons.delete.gif";
            public const string Calendar = "TeaCommerce.Umbraco.Application.Content.Images.EditorIcons.calSprite.gif";
        }

        public class TreeIcons
        {
            public const string BoxLabel = "icon-box";
            public const string Building = "icon-company";
            public const string Certificate = "icon-gift";
            public const string ClipboardTask = "icon-paste-in";
            public const string Clipboard = "icon-paste-in";
            public const string CreditCard = "icon-credit-card-alt";
            public const string CreditCards = "icon-multiple-credit-cards";
            public const string DocumentTask = "icon-invoice";
            public const string GlobeModel = "icon-globe-inverted-europe-africa";
            public const string LicenseKey = "icon-certificate";
            public const string Lifebuoy = "icon-help";
            public const string LocaleAlternate = "icon-umb-translation";
            public const string Lock = "icon-lock";
            public const string Mail = "icon-message";
            public const string MailStack = "icon-mailbox";
            public const string Map = "icon-flag-alt";
            public const string MapPin = "icon-map-location";
            public const string MoneyCoin = "icon-coins-euro-alt";
            public const string Money = "icon-coin-euro";
            public const string Store = "icon-store";
            public const string TagLabel = "icon-tag";
            public const string Target = "icon-target";
            public const string Toolbox = "icon-settings";
            public const string TruckBoxLabel = "icon-truck";
            public const string User = "icon-umb-users";
            public const string ZoneMoney = "icon-legal";
            public const string Zone = "icon-economy";
        }

        public class MiscIcons
        {
            public const string Exclamation = "TeaCommerce.Umbraco.Application.Content.Images.exclamation.png";
        }

        public class Scripts
        {
            public const string Default = "TeaCommerce.Umbraco.Application.Content.Scripts.default.js";
        }

        public class Pages
        {
            public const string EditOrder = "/Views/Orders/EditOrder.aspx";
            public const string FinalizeOrder = "/Views/Orders/FinalizeOrder.aspx";
            public const string SearchOrders = "/Views/Orders/SearchOrders.aspx";
            public const string NeedHelp = "/Views/Developer/NeedHelp.aspx";
            public const string LicenseCheck = "/Views/Developer/LicenseCheck.aspx";
            public const string EditStore = "/Views/Stores/EditStore.aspx";
            public const string EditCampaign = "/Views/Campaigns/EditCampaign.aspx";
            public const string EditCampaignItem = "/Views/Campaigns/EditCampaignItem.aspx";
            public const string GiftCardOverview = "/Views/GiftCards/GiftCardOverview.aspx";
            public const string EditGiftCard = "/Views/GiftCards/EditGiftCard.aspx";
            public const string EditOrderStatus = "/Views/OrderStatuses/EditOrderStatus.aspx";
            public const string EditShippingMethod = "/Views/ShippingMethods/EditShippingMethod.aspx";
            public const string EditPaymentMethod = "/Views/PaymentMethods/EditPaymentMethod.aspx";
            public const string EditCountry = "/Views/Countries/EditCountry.aspx";
            public const string EditCountryRegion = "/Views/CountryRegions/EditCountryRegion.aspx";
            public const string EditCurrency = "/Views/Currencies/EditCurrency.aspx";
            public const string EditVatGroup = "/Views/VatGroups/EditVatGroup.aspx";
            public const string EditEmailTemplate = "/Views/EmailTemplates/EditEmailTemplate.aspx";
            public const string EditUserPermissions = "/Views/Security/EditUserPermissions.aspx";
            public const string Sort = "/Views/Sort/Sort.aspx";
            public const string CreateAll = "/Views/CreateAll/CreateAll.aspx";
        }

        public class MimeTypes
        {
            public const string PNG = "image/png";
            public const string GIF = "image/gif";
            public const string CSS = "text/css";
            public const string JavaScript = "text/javascript";
        }

        public class DistributedCache
        {
            public const string StoreCacheRefresherId = "a0646f66-57bb-4990-a0ea-786883105db1";
            public const string OrderCacheRefresherId = "a3b1db6a-48f2-4c2c-9114-8ce7922e376e";
            public const string OrderStatusCacheRefresherId = "b5742f24-bfd7-451c-a841-70e4badaab48";
            public const string CountryCacheRefresherId = "964cfca6-c2a2-482d-87fb-4ebfaf0a35ac";
            public const string CountryRegionCacheRefresherId = "13477e30-0889-425d-a66b-b50b3a87c464";
            public const string CurrencyCacheRefresherId = "3b3e0ab7-5933-4a6d-93a7-6a83fbe727f7";
            public const string PaymentMethodCacheRefresherId = "0355df34-8dd6-4757-859f-e7d7c0643799";
            public const string ShippingMethodCacheRefresherId = "d8080850-a586-4ff4-ba2e-08c9945cb739";
            public const string VatGroupCacheRefresherId = "7cdf4e3b-6f8f-4f2c-8d9e-ee5f160d1fa6";
            public const string CampaignCacheRefresherId = "a2ab6518-a287-492a-86a8-4c24d7ac0dad";
            public const string EmailTemplateCacheRefresherId = "cbd44196-a1ba-4e93-9543-f0931d448d75";
            public const string PingServiceCacheRefresherId = "aa0a1be8-d0f8-4a7a-961f-e2670a1dc666";

            public static readonly Guid StoreCacheRefresherGuid = new Guid(StoreCacheRefresherId);
            public static readonly Guid OrderCacheRefresherGuid = new Guid(OrderCacheRefresherId);
            public static readonly Guid OrderStatusCacheRefresherGuid = new Guid(OrderStatusCacheRefresherId);
            public static readonly Guid CountryCacheRefresherGuid = new Guid(CountryCacheRefresherId);
            public static readonly Guid CountryRegionCacheRefresherGuid = new Guid(CountryRegionCacheRefresherId);
            public static readonly Guid CurrencyCacheRefresherGuid = new Guid(CurrencyCacheRefresherId);
            public static readonly Guid PaymentMethodCacheRefresherGuid = new Guid(PaymentMethodCacheRefresherId);
            public static readonly Guid ShippingMethodCacheRefresherGuid = new Guid(ShippingMethodCacheRefresherId);
            public static readonly Guid VatGroupCacheRefresherGuid = new Guid(VatGroupCacheRefresherId);
            public static readonly Guid CampaignCacheRefresherGuid = new Guid(CampaignCacheRefresherId);
            public static readonly Guid EmailTemplateCacheRefresherGuid = new Guid(EmailTemplateCacheRefresherId);
            public static readonly Guid PingServiceCacheRefresherGuid = new Guid(PingServiceCacheRefresherId);
        }

    }
}