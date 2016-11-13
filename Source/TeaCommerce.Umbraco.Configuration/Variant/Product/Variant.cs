using System.Collections.Generic;

namespace TeaCommerce.Umbraco.Configuration.Variant.Product {
  public class Variant {
    public string Id { get; set; }
    public Dictionary<string, string> Properties { get; set; }
    public List<Combination> Combination { get; set; }
    public string DocumentTypeAlias { get; set; }
    public VariantValidation Validation { get; set; }
  }

  public class VariantValidation {
    public bool DuplicatesFound { get; set; }
    public bool HolesInVariants { get; set; }
    
  }
}