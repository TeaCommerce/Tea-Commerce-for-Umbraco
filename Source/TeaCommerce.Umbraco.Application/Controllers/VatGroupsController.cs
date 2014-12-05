using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Xml.XPath;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using umbraco;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class VatGroupsController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage GetAll( string pageId ) {
      XPathNavigator xPathNavigator = library.GetXmlNodeById( pageId ).Current;
      IXmlNodeProductInformationExtractor productInformationExtractor = XmlNodeProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( xPathNavigator, false );

      if ( !productInformationExtractor.HasAccess( storeId, xPathNavigator, false ) ) {
        throw new HttpResponseException( HttpStatusCode.Forbidden );
      }

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage Get( string pageId, long vatGroupId ) {
      XPathNavigator xPathNavigator = library.GetXmlNodeById( pageId ).Current;
      IXmlNodeProductInformationExtractor productInformationExtractor = XmlNodeProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( xPathNavigator, false );

      if ( !productInformationExtractor.HasAccess( storeId, xPathNavigator, false ) ) {
        throw new HttpResponseException( HttpStatusCode.Forbidden );
      }

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.Get( storeId, vatGroupId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

  }
}