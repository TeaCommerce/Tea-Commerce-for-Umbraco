using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class VATGroupTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      VatGroup vatGroup = VatGroupService.Instance.Get( storeId, entityId );
      return vatGroup.Delete();
    }

  }
}