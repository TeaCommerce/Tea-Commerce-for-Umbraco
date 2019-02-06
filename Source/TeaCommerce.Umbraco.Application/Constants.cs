using System;

namespace TeaCommerce.Umbraco.Application
{
    public class Constants
    {
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
            public const string StoreCacheRefresherId = "15f51412-8255-446a-aef0-000815895d08";
            public const string OrderCacheRefresherId = "49134a22-48b1-41d4-aeae-163773c7a348";
            public const string OrderStatusCacheRefresherId = "3689cc61-f62f-4c5f-8a2d-34d218181b4f";
            public const string CountryCacheRefresherId = "867e49c4-4f65-42ae-beae-95fe27441d5b";
            public const string CountryRegionCacheRefresherId = "c6904c5a-9977-4f2d-ab07-28deec610f24";
            public const string CurrencyCacheRefresherId = "068e852b-242d-4487-9f9d-b54475e623a8";
            public const string PaymentMethodCacheRefresherId = "e8ade12a-0707-489a-bb3a-9ecda8fa9987";
            public const string ShippingMethodCacheRefresherId = "b70d2347-416c-43ae-a0d8-84fc69600b49";
            public const string VatGroupCacheRefresherId = "3c033031-955e-4757-8948-fbe70ae7f6fc";
            public const string CampaignCacheRefresherId = "8c0b431e-2301-4a39-b822-bc8ce687d210";
            public const string EmailTemplateCacheRefresherId = "f36575b6-c003-41ea-b842-e6d73a610ea7";
            public const string PingServiceCacheRefresherId = "5549c944-080a-4c11-98ec-e1dfb3ae54a7";

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