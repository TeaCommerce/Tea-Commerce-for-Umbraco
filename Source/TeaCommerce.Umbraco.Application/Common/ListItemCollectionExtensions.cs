
namespace System.Web.UI.WebControls {
  public static class ListItemCollectionExtensions {

    public static bool TrySelectByValue( this ListItemCollection collection, object value ) {
      if ( value == null || collection == null )
        return false;
      ListItem item = collection.FindByValue( value.ToString() );
      if ( item != null ) {
        return ( item.Selected = true );
      }
      return false;
    }

  }
}