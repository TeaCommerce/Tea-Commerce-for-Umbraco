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

    [HttpGet]
    public Stock GetStock( string pageId ) {
      Stock stock = new Stock();

      XPathNavigator xPathNavigator = library.GetXmlNodeById( pageId ).Current;
      IXmlNodeProductInformationExtractor productInformationExtractor = XmlNodeProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( xPathNavigator, false );

      stock.Sku = productInformationExtractor.GetSku( xPathNavigator, false );
      decimal? stockValue = ProductService.Instance.GetStock( storeId, stock.Sku );
      stock.Value = stockValue != null ? stockValue.Value.ToString( "0.####" ) : "";

      return stock;
    }

    [HttpPost]
    public void PostStock( string pageId, Stock stock ) {
      XPathNavigator xPathNavigator = library.GetXmlNodeById( pageId ).Current;
      IXmlNodeProductInformationExtractor productInformationExtractor = XmlNodeProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( xPathNavigator, false );
      stock.Sku = !string.IsNullOrEmpty( stock.Sku ) ? stock.Sku : productInformationExtractor.GetSku( xPathNavigator, false );

      ProductService.Instance.SetStock( storeId, stock.Sku, !string.IsNullOrEmpty( stock.Value ) ? stock.Value.ParseToDecimal() : null );
    }

  }
}