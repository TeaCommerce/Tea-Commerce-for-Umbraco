using System.Linq;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class GrantPermissions : IPackageAction {

    #region IPackageAction Members

    public string Alias() {
      return "GrantPermissions";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      //Giving permissions at this point won't work as the current user is null
      //User is given permissions when granting access to the Tea Commerce section
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