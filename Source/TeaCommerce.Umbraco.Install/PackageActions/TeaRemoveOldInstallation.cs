using System.Collections.Generic;
using System.Linq;
using System.Xml;
using umbraco.cms.businesslogic.packager;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class TeaRemoveOldInstallation : IPackageAction {

    #region IPackageAction Members

    public string Alias() {
      return "TeaRemoveOldInstallation";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      List<InstalledPackage> teaCommercePackages = InstalledPackage.GetAllInstalledPackages().Where( ip => ip.Data.Name.Equals( "Tea Commerce" ) || ip.Data.Name.Equals( "teacommerce" ) ).ToList();

      if ( teaCommercePackages.Count > 1 ) {
        teaCommercePackages.RemoveAt( teaCommercePackages.Count - 1 );
        foreach ( InstalledPackage package in teaCommercePackages )
          package.Delete();

      }
      return true;
    }

    public System.Xml.XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""{0}""></Action>", this.Alias() ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      return true;
    }

    #endregion

  }
}