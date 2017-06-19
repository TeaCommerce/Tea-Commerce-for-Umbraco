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


namespace TeaCommerce.Umbraco.Application2.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class CountryController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long countryId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( CountryService.Instance.Get( storeId, countryId ).ToJson() )
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
    public HttpResponseMessage GetShippingMethods( long storeId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( ShippingMethodService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetPaymentMethods( long storeId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( PaymentMethodService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpPost]
    public bool Save( Country countryDTO ) {
      Country country = CountryService.Instance.Get( countryDTO.StoreId, countryDTO.Id );
      country.Name = countryDTO.Name;
      country.RegionCode = countryDTO.RegionCode;
      country.DefaultCurrencyId = countryDTO.DefaultCurrencyId;
      country.DefaultShippingMethodId = countryDTO.DefaultShippingMethodId;
      country.DefaultPaymentMethodId = countryDTO.DefaultPaymentMethodId;
      country.Save();

      return true;
    }
  }
}
