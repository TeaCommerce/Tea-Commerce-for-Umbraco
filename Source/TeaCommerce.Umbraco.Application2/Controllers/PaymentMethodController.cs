using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Api.Services;
using Umbraco.Web.Editors;
using TeaCommerce.Umbraco.Application2;
using Umbraco.Web.Mvc;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class PaymentMethodController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long paymentMethodId ) {
      
      HttpResponseMessage response = new HttpResponseMessage {
        //copy to dto
        Content = new StringContent( PaymentMethodService.Instance.Get( storeId, paymentMethodId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetVatGroups( long storeId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetCurrencies( long storeId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( CurrencyService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetCountries( long storeId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( CountryService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetCountryRegions( long storeId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( CountryRegionService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpPost]
    public bool Save( PaymentMethod PaymentMethodDTO ) {
      PaymentMethod paymentMethod = PaymentMethodService.Instance.Get( PaymentMethodDTO.StoreId, PaymentMethodDTO.Id );
      paymentMethod.Name = PaymentMethodDTO.Name;
      paymentMethod.Alias = PaymentMethodDTO.Alias;
      paymentMethod.Sku = PaymentMethodDTO.Sku;
      paymentMethod.VatGroupId = PaymentMethodDTO.VatGroupId;
      paymentMethod.ImageIdentifier = PaymentMethodDTO.ImageIdentifier;
      paymentMethod.Save();

      return true;
    }

  }
}