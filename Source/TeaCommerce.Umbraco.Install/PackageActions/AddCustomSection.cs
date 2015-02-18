using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class AddCustomSection : IPackageAction {

    #region IPackageAction Members

    public string Alias() {
      return "AddCustomSection";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      if ( ApplicationContext.Current.Services.SectionService.GetByAlias( "teacommerce" ) == null ) {
        ApplicationContext.Current.Services.SectionService.MakeNew( "Tea Commerce", "teacommerce", "icon-shopping-basket-alt-2" );
      }
      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( @"<Action runat=""install"" alias=""AddCustomSection"" />" );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      ApplicationContext.Current.Services.SectionService.DeleteSection( ApplicationContext.Current.Services.SectionService.GetByAlias( "teacommerce" ) );
      return true;
    }

    #endregion
  }
}