using Newtonsoft.Json;
using System.Collections.Generic;
using TeaCommerce.Api.Marketing.Models.Awards;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Models.Awards {

  [Award( "OrderLineAmountAward" )]
  public class OrderLineAmountAward : AOrderLineAmountAward {

    public int? NodeId { get; set; }

    private readonly IProductService _productService;

    public OrderLineAmountAward() {
      _productService = ProductService.Instance;
    }

    public override void LoadSettings() {
      base.LoadSettings();

      var settings = JsonConvert.DeserializeAnonymousType( Settings, new { NodeId = (int?)null } );

      if ( settings == null ) return;

      if ( settings.NodeId != 0 ) {
        NodeId = settings.NodeId;
      }
    }

    public override IEnumerable<OrderLine> FilterOrderLines( Order order, IEnumerable<OrderLine> orderLines ) {
      return NodeId == null ? orderLines : ProductUtils.OrderLinesThatMatchProductOrProductCategory( _productService, NodeId.Value, orderLines );
    }
  }
}