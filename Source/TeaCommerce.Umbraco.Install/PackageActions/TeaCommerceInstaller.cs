using System.Xml;
using TeaCommerce.Api.Infrastructure.Installation;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class TeaCommerceInstaller : IPackageAction {

    #region IPackageAction Members

    public string Alias() {
      return "TeaCommerceInstaller";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      InstallationService.Instance.InstallOrUpdate();
      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""{0}"" />", Alias() ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      InstallationService.Instance.Uninstall();
      return true;
    }

    #endregion

  }
}