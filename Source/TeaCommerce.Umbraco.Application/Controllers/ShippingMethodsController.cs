using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Api.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class ShippingMethodsController : UmbracoAuthorizedApiController {

    [HttpGet]
    public HttpResponseMessage GetAll( long storeId ) {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( ShippingMethodService.Instance.GetAll( storeId ).ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long shippingMethodId ) {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( ShippingMethodService.Instance.Get( storeId, shippingMethodId ).ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

  }
}