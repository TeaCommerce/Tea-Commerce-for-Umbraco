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
  public class CountryRegionController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long countryRegionId ) {
      
      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( CountryRegionService.Instance.Get( storeId, countryRegionId ).ToJson() )
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
    public bool Save( CountryRegion countryRegionDTO ) {
      CountryRegion countryRegion = CountryRegionService.Instance.Get( countryRegionDTO.StoreId, countryRegionDTO.Id );
      countryRegion.Name = countryRegionDTO.Name;
      countryRegion.RegionCode = countryRegionDTO.RegionCode;
      countryRegion.DefaultShippingMethodId = countryRegionDTO.DefaultShippingMethodId;
      countryRegion.DefaultPaymentMethodId = countryRegionDTO.DefaultPaymentMethodId;
      countryRegion.Save();

      return true;
    }

  }
}