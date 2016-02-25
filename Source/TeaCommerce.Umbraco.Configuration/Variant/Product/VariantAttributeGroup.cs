using System.Collections.Generic;

namespace TeaCommerce.Umbraco.Configuration.Variant.Product {
  public class VariantAttributeGroup {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<VariantAttribute> Attributes { get; set; }

    public VariantAttributeGroup() {
      Attributes = new List<VariantAttribute>();
    }
  }
}
