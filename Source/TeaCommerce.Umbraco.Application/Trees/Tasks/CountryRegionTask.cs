using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class CountryRegionTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      CountryRegion countryRegion = CountryRegionService.Instance.Get( storeId, entityId );
      return countryRegion.Delete();
    }

  }
}