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
  public class PaymentMethodsController : UmbracoAuthorizedApiController {

    [HttpGet]
    public HttpResponseMessage GetAll( long storeId ) {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( PaymentMethodService.Instance.GetAll( storeId ).ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long paymentMethodId ) {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( PaymentMethodService.Instance.Get( storeId, paymentMethodId ).ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

  }
}