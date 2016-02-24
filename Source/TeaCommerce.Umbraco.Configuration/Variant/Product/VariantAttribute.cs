using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Variant.Product {
  public class VariantAttribute {
    public int Id { get; private set; }
    public string Name { get; private set; }

    public VariantAttribute( IPublishedContent content ) {
      Id = content.Id;
      Name = content.Name;
    }
  }
}
