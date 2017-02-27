using System.Collections.Generic;

namespace TeaCommerce.Umbraco.Configuration.Variants.Models {
  public interface IVariant {
    string VariantIdentifier { get; }
    List<Specification> Combination { get; }
  }
}
