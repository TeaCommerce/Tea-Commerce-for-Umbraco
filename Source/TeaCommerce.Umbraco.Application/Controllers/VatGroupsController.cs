using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Xml.XPath;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class VatGroupsController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage GetAll( string pageId ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( pageId );
      IContent content = ApplicationContext.Current.Services.ContentService.GetById( productIdentifierObj.NodeId );
      IProductInformationExtractor<IContent, VariantPublishedContent<IContent>> productInformationExtractor = ContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content );

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage Get( string pageId, long vatGroupId ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( pageId );
      IContent content = ApplicationContext.Current.Services.ContentService.GetById( productIdentifierObj.NodeId );
      IProductInformationExtractor<IContent, VariantPublishedContent<IContent>> productInformationExtractor = ContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content );

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( VatGroupService.Instance.Get( storeId, vatGroupId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

  }
}