using System.Web.Http;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core;
using Umbraco.Core.Models;
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
    public Stock GetStock( string productIdentifier ) {
      
      Stock stock = new Stock();
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );

      IContent content = ApplicationContext.Current.Services.ContentService.GetById( productIdentifierObj.NodeId );
      IContentProductInformationExtractor productInformationExtractor = ContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content );

      stock.Sku = productInformationExtractor.GetSku( content, productIdentifierObj.VariantId );
      decimal? stockValue = ProductService.Instance.GetStock( storeId, stock.Sku );
      stock.Value = stockValue != null ? stockValue.Value.ToString( "0.####" ) : "";

      return stock;
    }

    [HttpPost]
    public void PostStock( string productIdentifier, Stock stock ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );

      IContent content = ApplicationContext.Current.Services.ContentService.GetById( productIdentifierObj.NodeId );
      IContentProductInformationExtractor productInformationExtractor = ContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content );

      stock.Sku = !string.IsNullOrEmpty( stock.Sku ) ? stock.Sku : productInformationExtractor.GetSku( content, productIdentifierObj.VariantId );

      ProductService.Instance.SetStock( storeId, stock.Sku, !string.IsNullOrEmpty( stock.Value ) ? stock.Value.ParseToDecimal() : null );
    }


  }
}