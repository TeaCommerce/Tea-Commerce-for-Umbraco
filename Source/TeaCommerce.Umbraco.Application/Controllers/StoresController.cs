using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Api.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class StoresController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage GetAll() {
      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( StoreService.Instance.GetAll().Where( store => permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessStore, store.Id ) ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage Get( long storeId ) {
      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

      if ( permissions == null || !permissions.HasPermission( StoreSpecificPermissionType.AccessStore, storeId ) ) {
        throw new HttpResponseException( HttpStatusCode.Forbidden );
      }

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( StoreService.Instance.Get( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

  }
}