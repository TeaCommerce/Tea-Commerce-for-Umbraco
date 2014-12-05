using System.Xml;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.DataLayer;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class AddCustomSection : IPackageAction {

    private string appName;
    private string appAlias;
    private string appIcon;

    public void Initialize( XmlNode xmlData ) {
      appName = xmlData.Attributes[ "appName" ].Value;
      appAlias = xmlData.Attributes[ "appAlias" ].Value;
      appIcon = xmlData.Attributes[ "appIcon" ].Value;
    }

    #region IPackageAction Members

    public string Alias() {
      return "AddCustomSection";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      umbraco.BusinessLogic.Application.MakeNew( appName, appAlias, appIcon );
      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""{0}"" appName=""TeaCommerce"" appAlias=""teacommerce"" icon=""teacommerce.png""/>", this.Alias() ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      IParameter appAliasParam = umbraco.BusinessLogic.Application.SqlHelper.CreateParameter( "@AppAlias", appAlias );
      umbraco.BusinessLogic.Application.SqlHelper.ExecuteNonQuery( "DELETE FROM umbracoUser2app WHERE app = @AppAlias", new IParameter[] { appAliasParam } );

      foreach ( ApplicationTree tree in ApplicationTree.getApplicationTree( appAlias ) )
        tree.Delete();

      umbraco.BusinessLogic.Application.getByAlias( appAlias ).Delete();
      return true;
    }

    #endregion
  }
}