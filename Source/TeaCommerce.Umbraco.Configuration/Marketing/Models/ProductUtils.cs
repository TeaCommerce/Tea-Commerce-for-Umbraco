using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeaCommerce.Api;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Models {
  public static class ProductUtils {
    public static IEnumerable<OrderLine> OrderLinesThatMatchProductOrProductCategory( IProductService productService, int nodeId, IEnumerable<OrderLine> orderLines ) {
      List<OrderLine> tempOrderLines = new List<OrderLine>();
      string nodeIdStr = nodeId.ToString( CultureInfo.InvariantCulture );
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );

      foreach ( OrderLine orderLine in orderLines ) {
        if ( productService.GetSku( nodeIdStr ) == orderLine.Sku ) {
          tempOrderLines.Add( orderLine );
          continue;
        }

        ProductIdentifier productIdentifierObj = new ProductIdentifier( orderLine.ProductIdentifier );

        IPublishedContent productContent = umbracoHelper.TypedContent( productIdentifierObj.NodeId );

        //Check the path - it could be a "product category" that was selected
        if ( productContent.Path.Split( new[] { ',' }, StringSplitOptions.None ).Contains( nodeIdStr ) ) {
          tempOrderLines.Add( orderLine );
          continue;
        }

        //Test if the master relation could be a "product category" that was selected
        string masterRelationNodeId = productContent.GetPropertyValue<string>( Constants.ProductPropertyAliases.MasterRelationPropertyAlias );

        if ( string.IsNullOrEmpty( masterRelationNodeId ) ) continue;

        IPublishedContent masterRelationNode = umbracoHelper.TypedContent( masterRelationNodeId );

        if ( masterRelationNode.Path.Split( new[] { ',' }, StringSplitOptions.None ).Contains( nodeIdStr ) ) {
          tempOrderLines.Add( orderLine );
        }
      }

      return tempOrderLines;
    }
  }
}