
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
    }

    public class Views {
      public const string CreateStore = "/teacommerce/";// "/App_Plugins/" + Applications.TeaCommerce + "/backoffice/stores/create.html";
      public const string EditStore = "/App_Plugins/" + Applications.TeaCommerce + "/backoffice/stores/edit.html";
    }

  }
}
