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
  public class ShippingMethodController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long shippingMethodId ) {
      
      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( ShippingMethodService.Instance.Get( storeId, shippingMethodId ).ToJson() )
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

    [HttpPost]
    public bool Save( ShippingMethod shippingMethodDTO ) {
      ShippingMethod shippingMethod = ShippingMethodService.Instance.Get( shippingMethodDTO.StoreId, shippingMethodDTO.Id );
      shippingMethod.Name = shippingMethodDTO.Name;
      shippingMethod.Alias = shippingMethodDTO.Alias;
      shippingMethod.Sku = shippingMethodDTO.Sku;
      shippingMethod.VatGroupId = shippingMethodDTO.VatGroupId;
      shippingMethod.ImageIdentifier = shippingMethodDTO.ImageIdentifier;
      shippingMethod.Save();

      return true;
    }

  }
}