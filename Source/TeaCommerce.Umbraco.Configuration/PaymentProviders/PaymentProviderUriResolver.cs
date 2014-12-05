using System;
using System.Web;
using TeaCommerce.Api.Web.PaymentProviders;

namespace TeaCommerce.Umbraco.Configuration.PaymentProviders {
  public class PaymentProviderUriResolver : IPaymentProviderUriResolver {

    public string GetContinueUrl( long storeId, string paymentProviderAlias, Guid orderId, string hash ) {
      return GetUrl( "PaymentContinue", storeId, paymentProviderAlias, orderId, hash );
    }

    public string GetCancelUrl( long storeId, string paymentProviderAlias, Guid orderId, string hash ) {
      return GetUrl( "PaymentCancel", storeId, paymentProviderAlias, orderId, hash );
    }

    public string GetCallbackUrl( long storeId, string paymentProviderAlias, Guid orderId, string hash ) {
      return GetUrl( "PaymentCallback", storeId, paymentProviderAlias, orderId, hash );
    }

    public string GetCommunicationUrl( long storeId, string paymentProviderAlias, Guid orderId, string hash ) {
      return GetUrl( "PaymentCommunication", storeId, paymentProviderAlias, orderId, hash );
    }

    private string GetUrl( string method, long storeId, string paymentProviderAlias, Guid orderId, string hash ) {
      HttpRequest request = HttpContext.Current.Request;
      Uri baseUrl = new UriBuilder( request.Url.Scheme, request.Url.Host, request.Url.Port ).Uri;

      return new Uri( baseUrl, "/base/TC/" + method + "/" + storeId + "/" + paymentProviderAlias + "/" + orderId + "/" + hash + ".aspx" ).AbsoluteUri;
    }

    
  }
}
