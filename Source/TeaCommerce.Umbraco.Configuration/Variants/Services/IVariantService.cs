using System.Collections.Generic;
using TeaCommerce.Umbraco.Configuration.Variants.Models;

namespace TeaCommerce.Umbraco.Configuration.Variants.Services {
  public interface IVariantService<T, T2> where T2 : IVariant {

    T2 GetVariant( long storeId, T product, string variantId, bool onlyValid = true );
    IEnumerable<T2> GetVariants( long storeId, T product, bool onlyValid = true );
    IEnumerable<VariantGroup> GetVariantGroups( IEnumerable<T2> variants );
    string GetVariantJson( long storeId, IEnumerable<T> productContents, bool onlyValid = true );
  }
}