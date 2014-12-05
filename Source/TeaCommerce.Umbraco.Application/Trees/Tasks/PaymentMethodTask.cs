using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class PaymentMethodTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      PaymentMethod paymentMethod = PaymentMethodService.Instance.Get( storeId, entityId );
      return paymentMethod.Delete();
    }

  }
}