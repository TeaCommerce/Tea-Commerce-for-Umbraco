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
      //Give access if no stores is created
      if ( !StoreService.Instance.GetAll().Any() ) {
        IUser user = ApplicationContext.Current.Services.UserService.GetUserById( UmbracoContext.Current.Security.GetUserId() );
        if ( !user.AllowedSections.Contains( "teacommerce" ) ) {
          user.AddAllowedSection( "teacommerce" );
          ApplicationContext.Current.Services.UserService.Save( user );
        }

        //If your not the super admin - give access to the Tea Commerce default features
        Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
        if ( permissions != null && !permissions.IsUserSuperAdmin ) {
          permissions.GeneralPermissions |= GeneralPermissionType.AccessSecurity;
          permissions.GeneralPermissions |= GeneralPermissionType.AccessLicenses;
          permissions.GeneralPermissions |= GeneralPermissionType.CreateAndDeleteStore;

          permissions.Save();
        }
      }
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
