using System.Linq;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Services;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class GrantPermissionsInstallTask : IInstallTask {

    public GrantPermissionsInstallTask() {
    }

    public void Install() {
      //Giving permissions at this point won't work as the current user is null
      //User is given permissions when granting access to the Tea Commerce section
    }

    public void Uninstall() {
      int totalRecords;
      foreach ( IUser user in ApplicationContext.Current.Services.UserService.GetAll( 0, 10000, out totalRecords ) ) {
        if ( user.AllowedSections.Contains( "teacommerce" ) ) {
          user.RemoveAllowedSection( "teacommerce" );
          ApplicationContext.Current.Services.UserService.Save( user );
        }
      }
    }
  }
}
