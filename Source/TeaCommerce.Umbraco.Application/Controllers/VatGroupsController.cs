using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Xml.XPath;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class VatGroupsController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage GetAll( string pageId ) {
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );
      ProductIdentifier productIdentifierObj = new ProductIdentifier( pageId );
      IPublishedContent content = umbracoHelper.TypedContent( productIdentifierObj.NodeId );
      IIPublishedContentProductInformationExtractor productInformationExtractor = IPublishedContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content, false );

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage Get( string pageId, long vatGroupId ) {
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );
      ProductIdentifier productIdentifierObj = new ProductIdentifier( pageId );
      IPublishedContent content = umbracoHelper.TypedContent( productIdentifierObj.NodeId );
      IIPublishedContentProductInformationExtractor productInformationExtractor = IPublishedContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content, false );

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.Get( storeId, vatGroupId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

  }
}