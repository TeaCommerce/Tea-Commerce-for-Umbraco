
namespace TeaCommerce.Umbraco.Configuration.Variant.Product {
  public class ProductIdentifier {

    public int NodeId { get; set; }
    public string VariantId { get; set; }

    public ProductIdentifier( string productIdentifier ) {
      if ( productIdentifier.Contains( "_" ) ) {
        string nodeIdStr = productIdentifier.Split( '_' )[ 0 ];
        int nodeId;
        int.TryParse( nodeIdStr, out nodeId );
        VariantId = productIdentifier.Split( '_' )[ 1 ];
      } else {
        string nodeIdStr = productIdentifier;
        int nodeId;
        int.TryParse( nodeIdStr, out nodeId );
      }
    }
  }

}
