using System;
using System.Collections.Generic;
using System.Globalization;
using TeaCommerce.Api;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using System.Linq;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Models {
  public static class ProductUtils {
    public static IEnumerable<OrderLine> OrderLinesThatMatchProductOrProductCategory( IProductService productService, int nodeId, IEnumerable<OrderLine> orderLines ) {
      List<OrderLine> tempOrderLines = new List<OrderLine>();
      string nodeIdStr = nodeId.ToString( CultureInfo.InvariantCulture );

      foreach ( OrderLine orderLine in orderLines ) {
        if ( productService.GetSku( nodeIdStr ) == orderLine.Sku ) {
          tempOrderLines.Add( orderLine );
          continue;
        }

        //Check the path - it could be a "product category" that was selected
        if ( productService.GetPropertyValue( orderLine.ProductIdentifier, "@path" ).Split( new[] { ',' }, StringSplitOptions.None ).Contains( nodeIdStr ) ) {
          tempOrderLines.Add( orderLine );
          continue;
        }

        //Test if the master relation chould be a "product category" that was selected
        string masterRelationNodeId = productService.GetPropertyValue( orderLine.ProductIdentifier, Constants.ProductPropertyAliases.MasterRelationPropertyAlias );
        if ( string.IsNullOrEmpty( masterRelationNodeId ) ) continue;

        if ( productService.GetPropertyValue( masterRelationNodeId, "@path" ).Split( new[] { ',' }, StringSplitOptions.None ).Contains( nodeIdStr ) ) {
          tempOrderLines.Add( orderLine );
        }
      }

      return tempOrderLines;
    }
  }
}