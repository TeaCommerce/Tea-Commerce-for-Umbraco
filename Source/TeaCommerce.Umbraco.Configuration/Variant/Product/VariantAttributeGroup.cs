using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Variant.Product {
  public class VariantAttributeGroup {
    public string Name { get; private set; }
    public List<VariantAttribute> Attributes { get; private set; }

    public VariantAttributeGroup( IPublishedContent content ) {
      Name = content.Name;
      Attributes = content.Children.Select( c => new VariantAttribute( c ) ).ToList();
    }
  }
}
