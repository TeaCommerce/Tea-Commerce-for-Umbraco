
namespace TeaCommerce.Umbraco.Application.UI {
  public class StoreSpecificUserControl : UmbracoUserControl {

    public long StoreId { get { return long.Parse( Request.QueryString[ "nodeId" ].Split( '_' )[ 1 ] ); } }

  }
}