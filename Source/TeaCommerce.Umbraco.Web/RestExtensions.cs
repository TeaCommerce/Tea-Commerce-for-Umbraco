using TeaCommerce.Api.Web;
using Umbraco.Web.BaseRest;

namespace TeaCommerce.Umbraco.Web {

  [RestExtension( "TC" )]
  public class RestExtensions {

    [RestExtensionMethod( ReturnXml = false )]
    public static void PaymentContinueWithoutOrderId( long storeId, string paymentProviderAlias, long paymentMethodId ) {
      FormPostHandler.PaymentContinueWithoutOrderId( storeId, paymentProviderAlias, paymentMethodId );
    }

    [RestExtensionMethod( ReturnXml = false )]
    public static void PaymentContinue( long storeId, string paymentProviderAlias, string orderId, string hash ) {
      FormPostHandler.PaymentContinue( storeId, paymentProviderAlias, orderId, hash );
    }

    [RestExtensionMethod( ReturnXml = false )]
    public static void PaymentCancelWithoutOrderId( long storeId, string paymentProviderAlias, long paymentMethodId ) {
      FormPostHandler.PaymentCancelWithoutOrderId( storeId, paymentProviderAlias, paymentMethodId );
    }

    [RestExtensionMethod( ReturnXml = false )]
    public static void PaymentCancel( long storeId, string paymentProviderAlias, string orderId, string hash ) {
      FormPostHandler.PaymentCancel( storeId, paymentProviderAlias, orderId, hash );
    }

    [RestExtensionMethod( ReturnXml = false )]
    public static void PaymentCallbackWithoutOrderId( long storeId, string paymentProviderAlias, long paymentMethodId ) {
      FormPostHandler.PaymentCallbackWithoutOrderId( storeId, paymentProviderAlias, paymentMethodId );
    }

    [RestExtensionMethod( ReturnXml = false )]
    public static void PaymentCallback( long storeId, string paymentProviderAlias, string orderId, string hash ) {
      FormPostHandler.PaymentCallback( storeId, paymentProviderAlias, orderId, hash );
    }

    [RestExtensionMethod( ReturnXml = false )]
    public static string PaymentCommunicationWithoutOrderId( long storeId, string paymentProviderAlias, long paymentMethodId ) {
      return FormPostHandler.PaymentCommunicationWithoutOrderId( storeId, paymentProviderAlias, paymentMethodId );
    }

    [RestExtensionMethod( ReturnXml = false )]
    public static string PaymentCommunication( long storeId, string paymentProviderAlias, string orderId, string hash ) {
      return FormPostHandler.PaymentCommunication( storeId, paymentProviderAlias, orderId, hash );
    }

    /// <summary>
    /// The general form post method
    /// Can be called by either a JavaScript ajax call or a normal form post
    /// </summary>
    /// <returns>Will return json if called by a JavaScript and redirect to a specific url if called by a form post call</returns>
    [RestExtensionMethod( ReturnXml = false )]
    public static string FormPost() {
      return FormPostHandler.FormPost();
    }

  }
}
