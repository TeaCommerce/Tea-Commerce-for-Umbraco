using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class OrderStatusTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      OrderStatus orderStatus = OrderStatusService.Instance.Get( storeId, entityId );
      return orderStatus.Delete();
    }

  }
}