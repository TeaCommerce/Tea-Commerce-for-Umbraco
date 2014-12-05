using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class CountryTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      Country country = CountryService.Instance.Get( storeId, entityId );
      return country.Delete();
    }

  }
}