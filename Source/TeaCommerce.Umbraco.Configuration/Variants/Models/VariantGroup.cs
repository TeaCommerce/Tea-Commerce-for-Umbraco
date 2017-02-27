using System.Collections.Generic;

namespace TeaCommerce.Umbraco.Configuration.Variants.Models {
  public class VariantGroup {
    public string Id { get; set; }
    public string Name { get; set; }
    public List<VariantType> Attributes { get; set; }

    public VariantGroup() {
      Attributes = new List<VariantType>();
    }
  }
}
