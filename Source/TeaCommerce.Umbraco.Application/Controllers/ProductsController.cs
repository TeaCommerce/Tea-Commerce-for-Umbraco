﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Xml.XPath;
using TeaCommerce.Api.Common;
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
  public class ProductsController : UmbracoAuthorizedJsonController {

    public class Stock {
      public string Sku { get; set; }
      public string Value { get; set; }
    }

    [HttpGet]
    public Stock GetStock( string productIdentifier ) {
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );
      Stock stock = new Stock();
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );

      IPublishedContent content = umbracoHelper.TypedContent( productIdentifierObj.NodeId );
      IIPublishedContentProductInformationExtractor productInformationExtractor = IPublishedContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content, false );

      stock.Sku = productInformationExtractor.GetSku( content, productIdentifierObj.VariantId, false );
      decimal? stockValue = ProductService.Instance.GetStock( storeId, stock.Sku );
      stock.Value = stockValue != null ? stockValue.Value.ToString( "0.####" ) : "";

      return stock;
    }

    [HttpPost]
    public void PostStock( string productIdentifier, Stock stock ) {
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );

      IPublishedContent content = umbracoHelper.TypedContent( productIdentifierObj.NodeId );
      IIPublishedContentProductInformationExtractor productInformationExtractor = IPublishedContentProductInformationExtractor.Instance;

      long storeId = productInformationExtractor.GetStoreId( content, false );

      stock.Sku = !string.IsNullOrEmpty( stock.Sku ) ? stock.Sku : productInformationExtractor.GetSku( content, productIdentifierObj.VariantId, false );

      ProductService.Instance.SetStock( storeId, stock.Sku, !string.IsNullOrEmpty( stock.Value ) ? stock.Value.ParseToDecimal() : null );
    }


  }
}