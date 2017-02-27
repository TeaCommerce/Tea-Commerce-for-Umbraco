using Umbraco.Core;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class SectionInstallTask : IInstallTask {

    private readonly string _name;
    private readonly string _alias;
    private readonly string _icon;

    public SectionInstallTask( string name, string alias, string icon ) {
      _name = name;
      _alias = alias;
      _icon = icon;
    }

    public void Install() {
      if ( ApplicationContext.Current.Services.SectionService.GetByAlias( _alias ) == null ) {
        ApplicationContext.Current.Services.SectionService.MakeNew( _name, _alias, _icon );
      }
    }

    public void Uninstall() {
      ApplicationContext.Current.Services.SectionService.DeleteSection( ApplicationContext.Current.Services.SectionService.GetByAlias( _alias ) );
    }
  }
}
