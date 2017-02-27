using Umbraco.Core;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class ApplicationTreeInstallTask : IInstallTask {

    private readonly string _alias;
    private readonly string _title;
    private readonly byte _sortOrder;
    private readonly string _type;

    public ApplicationTreeInstallTask( string alias, string title, byte sortOrder, string type ) {
      _alias = alias;
      _title = title;
      _sortOrder = sortOrder;
      _type = type;
    }

    public void Install() {
      if ( ApplicationContext.Current.Services.ApplicationTreeService.GetByAlias( _alias ) == null ) {
        ApplicationContext.Current.Services.ApplicationTreeService.MakeNew( true, _sortOrder, "teacommerce", _alias, _title, "folder_o.gif", "folder.gif", _type );
      }
    }

    public void Uninstall() {
      ApplicationContext.Current.Services.ApplicationTreeService.DeleteTree( ApplicationContext.Current.Services.ApplicationTreeService.GetByAlias( _alias ) );
    }
  }
}
