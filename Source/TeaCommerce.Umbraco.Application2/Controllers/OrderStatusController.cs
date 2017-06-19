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
  public class OrderStatusController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long orderStatusId ) {
      
      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( OrderStatusService.Instance.Get( storeId, orderStatusId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpPost]
    public bool Save( OrderStatus orderStatusDTO ) {
      OrderStatus orderStatus = OrderStatusService.Instance.Get( orderStatusDTO.StoreId, orderStatusDTO.Id );
      orderStatus.Name = orderStatusDTO.Name;
      orderStatus.Alias = orderStatusDTO.Alias;
      orderStatus.RecalculateFinalizedOrder = orderStatusDTO.RecalculateFinalizedOrder;
      orderStatus.Save();

      return true;
    }

  }
}