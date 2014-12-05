using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.IO;

namespace TeaCommerce.Uninstaller.PackageActionsUninstall {
  public class TeaAddDashboardSection : IPackageAction {

    private XDocument dashboardConfig;
    protected XDocument DashboardConfig {
      get {
        if ( dashboardConfig == null )
          dashboardConfig = XDocument.Load( SystemFiles.DashboardConfig );
        return dashboardConfig;
      }
    }

    private string dashboardAlias;

    public void Initialize( XmlNode xmlData ) {
      dashboardAlias = xmlData.Attributes[ "dashboardAlias" ].Value;
    }

    #region IPackageAction Members

    public string Alias() {
      return "TeaAddDashboardSection";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );

      XDocument doc = XDocument.Load( new XmlNodeReader( xmlData ) );
      IEnumerable<XElement> sourceSections = doc.XPathSelectElements( "//section" );

      if ( sourceSections.Any() ) {
        XElement targetSectionsRoot = DashboardConfig.XPathSelectElement( "//dashBoard" );

        foreach ( XElement sourceSection in sourceSections ) {
          sourceSection.SetAttributeValue( "alias", dashboardAlias );

          //Get the extension if it already exists - else create it
          XElement targetSection = targetSectionsRoot.XPathSelectElement( "./section [@alias='" + dashboardAlias + "']" );

          if ( targetSection != null )
            targetSection.ReplaceNodes( sourceSection.Nodes() );
          else
            targetSectionsRoot.AddFirst( sourceSection );
        }

        DashboardConfig.Save( SystemFiles.DashboardConfig, SaveOptions.None );

      }

      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""{0}"" dashboardAlias=""TeaCommerceDashboardSection""><areas><area>teacommerce</area></areas><tab caption=""Common""><control>/usercontrols/Test.ascx</control></tab></Action>", this.Alias() ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );

      DashboardConfig.XPathSelectElements( "//section" ).Where( i => i.Attribute( "alias" ).Value.Equals( dashboardAlias ) ).Remove();

      DashboardConfig.Save( SystemFiles.DashboardConfig, SaveOptions.None );

      return true;
    }

    #endregion
  }
}