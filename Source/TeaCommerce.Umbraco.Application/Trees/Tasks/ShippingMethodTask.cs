using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class ShippingMethodTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      ShippingMethod shippingMethod = ShippingMethodService.Instance.Get( storeId, entityId );
      return shippingMethod.Delete();
    }

  }
}