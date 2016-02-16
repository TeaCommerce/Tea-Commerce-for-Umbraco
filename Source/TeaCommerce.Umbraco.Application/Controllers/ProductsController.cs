using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Xml.XPath;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using umbraco;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class ProductsController : UmbracoAuthorizedJsonController {

    public class Stock {
      public string Sku { get; set; }
      public string Value { get; set; }
    }

    public class ProductIdentifier {
      public string PageId { get; set; }
      public string VariantGuid { get; set; }

      public ProductIdentifier( string productIdentifier ) {
        if ( productIdentifier.Contains( "_" ) ) {
          PageId = productIdentifier.Split( '_' )[0];
          VariantGuid = productIdentifier.Split( '_' )[1];
        } else {
          PageId = productIdentifier;
        }
      }
    }

    [HttpGet]
    public Stock GetStock( string productIdentifier ) {
      Stock stock = new Stock();
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );

      XPathNavigator xPathNavigator = library.GetXmlNodeById( productIdentifierObj.PageId ).Current;
      IXmlNodeProductInformationExtractor productInformationExtractor = XmlNodeProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( xPathNavigator, false );

      stock.Sku = productInformationExtractor.GetSku( xPathNavigator, productIdentifierObj.VariantGuid, false );
      decimal? stockValue = ProductService.Instance.GetStock( storeId, stock.Sku );
      stock.Value = stockValue != null ? stockValue.Value.ToString( "0.####" ) : "";

      return stock;
    }

    [HttpPost]
    public void PostStock( string productIdentifier, Stock stock ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );

      XPathNavigator xPathNavigator = library.GetXmlNodeById( productIdentifierObj.PageId ).Current;
      IXmlNodeProductInformationExtractor productInformationExtractor = XmlNodeProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( xPathNavigator, false );
      stock.Sku = !string.IsNullOrEmpty( stock.Sku ) ? stock.Sku : productInformationExtractor.GetSku( xPathNavigator, productIdentifierObj.VariantGuid, false );

      ProductService.Instance.SetStock( storeId, stock.Sku, !string.IsNullOrEmpty( stock.Value ) ? stock.Value.ParseToDecimal() : null );
    }


  }
}