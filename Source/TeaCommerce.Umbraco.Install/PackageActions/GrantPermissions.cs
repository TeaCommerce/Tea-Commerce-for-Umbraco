using System.Linq;
using System.Xml;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Services;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class GrantPermissions : IPackageAction {

    private const string UNINSTALL_SQL = "DELETE FROM umbracoUser2app WHERE umbracoUser2App.app = 'teacommerce'";
    private const string REVOKE_SQL = "DELETE FROM umbracoUser2app WHERE umbracoUser2App.app = 'teacommerce' AND [user] IN ( SELECT umbracoUser.id FROM umbracoUser WHERE umbracoUser.userLogin = '{0}' )";
    private const string GRANT_SQL = "INSERT INTO umbracoUser2app ([user], app) SELECT id, 'teacommerce' FROM umbracoUser WHERE userLogin = '{0}'";

    private string userLogin;

    public void Initialize( XmlNode xmlData ) {
      userLogin = User.GetCurrent().LoginName;
    }

    #region IPackageAction Members

    public string Alias() {
      return "GrantPermissions";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      if ( !StoreService.Instance.GetAll().Any() ) {
        Revoke();
        Grant();
      }
      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""GrantPermissions"" />" ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      Application.SqlHelper.ExecuteNonQuery( UNINSTALL_SQL );
      return true;
    }

    #endregion

    private void Grant() {
      Application.SqlHelper.ExecuteNonQuery( string.Format( GRANT_SQL, userLogin ) );

      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( permissions != null && !permissions.IsUserSuperAdmin ) {
        permissions.GeneralPermissions |= GeneralPermissionType.AccessSecurity;
        permissions.GeneralPermissions |= GeneralPermissionType.AccessLicenses;
        permissions.GeneralPermissions |= GeneralPermissionType.CreateAndDeleteStore;

        permissions.Save();
      }

    }

    private void Revoke() {
      Application.SqlHelper.ExecuteNonQuery( string.Format( REVOKE_SQL, userLogin ) );
    }

  }
}