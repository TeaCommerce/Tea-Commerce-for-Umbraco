using System.Linq;
using System.Xml;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Services;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class GrantPermissions : IPackageAction {

    #region IPackageAction Members

    public string Alias() {
      return "GrantPermissions";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
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
      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""GrantPermissions"" />" ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      int totalRecords;
      foreach ( IUser user in ApplicationContext.Current.Services.UserService.GetAll( 0, 10000, out totalRecords ) ) {
        if ( user.AllowedSections.Contains( "teacommerce" ) ) {
          user.RemoveAllowedSection( "teacommerce" );
          ApplicationContext.Current.Services.UserService.Save( user );
        }
      }

      return true;
    }

    #endregion

  }
}