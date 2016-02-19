
namespace TeaCommerce.Umbraco.Configuration.Variant.Product {
  public class ProductIdentifier {
    public string NodeId { get; set; }
    public string VariantId { get; set; }

    public ProductIdentifier( string productIdentifier ) {
      if ( productIdentifier.Contains( "_" ) ) {
        NodeId = productIdentifier.Split( '_' )[ 0 ];
        VariantId = productIdentifier.Split( '_' )[ 1 ];
      } else {
        NodeId = productIdentifier;
      }
    }
  }

}
