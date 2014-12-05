using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class CurrencyTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      Currency currency = CurrencyService.Instance.Get( storeId, entityId );
      return currency.Delete();
    }

  }
}