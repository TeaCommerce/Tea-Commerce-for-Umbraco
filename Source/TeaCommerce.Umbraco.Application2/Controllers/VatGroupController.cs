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
  public class VatGroupController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long vatGroupId ) {
      
      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.Get( storeId, vatGroupId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpPost]
    public bool Save( VatGroup vatGroupDTO ) {
      ShippingMethod vatGroup = ShippingMethodService.Instance.Get( vatGroupDTO.StoreId, vatGroupDTO.Id );
      vatGroup.Name = vatGroupDTO.Name;
      vatGroup.Save();

      return true;
    }

  }
}