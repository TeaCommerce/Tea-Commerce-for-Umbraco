using System.Collections.Generic;

namespace TeaCommerce.Umbraco.Configuration.Variant.Product {
  public class VariantGroup {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<VariantType> Attributes { get; set; }

    public VariantGroup() {
      Attributes = new List<VariantType>();
    }
  }
}
