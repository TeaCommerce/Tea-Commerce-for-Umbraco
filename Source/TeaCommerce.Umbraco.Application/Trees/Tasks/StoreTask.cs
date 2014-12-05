using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class StoreTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      Store store = StoreService.Instance.Get( entityId );
      return store.Delete();
    }

  }
}