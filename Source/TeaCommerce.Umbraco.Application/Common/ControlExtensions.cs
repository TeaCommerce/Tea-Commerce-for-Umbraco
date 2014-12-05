
namespace System.Web.UI {
  public static class ControlExtensions {

    public static T FindControl<T>( this Control control, string id ) where T : Control {
      return (T)control.FindControl( id );
    }

  }
}
