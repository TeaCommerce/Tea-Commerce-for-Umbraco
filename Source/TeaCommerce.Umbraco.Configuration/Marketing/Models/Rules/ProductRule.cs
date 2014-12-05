using Newtonsoft.Json;
using System.Collections.Generic;
using TeaCommerce.Api.Marketing.Models.Rules;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Models.Rules {

  [Rule( "ProductRule" )]
  public class ProductRule : ARule, IOrderLineRule {

    public int? NodeId { get; set; }

    private readonly IProductService _productService;

    public ProductRule() {
      _productService = ProductService.Instance;
    }

    public override void LoadSettings() {
      var settings = JsonConvert.DeserializeAnonymousType( Settings, new { NodeId = (int?)null } );

      if ( settings == null ) return;

      NodeId = settings.NodeId;
    }

    public IEnumerable<OrderLine> IsFulfilledBy( Order order, IEnumerable<OrderLine> previouslyFulfilledOrderLines ) {
      return NodeId != null ? ProductUtils.OrderLinesThatMatchProductOrProductCategory( _productService, NodeId.Value, previouslyFulfilledOrderLines ) : new List<OrderLine>();
    }

  }
}