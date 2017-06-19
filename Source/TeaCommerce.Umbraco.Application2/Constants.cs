
namespace TeaCommerce.Umbraco.Application2 {
  public class Constants {

    public class Applications {
      public const string TeaCommerce = "teacommerce2";
    }

    public class Trees {
      public const string TeaCommerce = "teacommerce2";
      public const string Stores = "stores";
      public const string Store = "store";
      public const string Settings = "settings";
      public const string OrderStatuses = "orderStatuses";
      public const string OrderStatus = "orderStatus";
      public const string Countries = "countries";
      public const string Country = "country";
      public const string CountryRegion = "countryRegion";
      public const string Currencies = "currencies";
      public const string Currency = "currency";
      public const string ShippingMethods = "shippingMethods";
      public const string ShippingMethod = "shippingMethod";
      public const string PaymentMethods = "paymentMethods";
      public const string PaymentMethod = "paymentMethod";
      public const string VatGroups = "vatGroups";
      public const string VatGroup = "vatGroup";
      public const string Internationalization = "internationalization";
    }

    public class Views {
      public const string CreateStore = "/teacommerce/";// "/App_Plugins/" + Applications.TeaCommerce + "/backoffice/stores/create.html";
      public const string EditStore = "/App_Plugins/" + Applications.TeaCommerce + "/backoffice/stores/edit.html";
    }

  }
}
