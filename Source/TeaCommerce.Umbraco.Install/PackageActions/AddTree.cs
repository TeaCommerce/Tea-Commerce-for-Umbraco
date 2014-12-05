using System.Linq;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class AddTree : IPackageAction {

    private bool silent;
    private bool initialize;
    private byte sortOrder;

    private string applicationAlias;
    private string treeAlias;
    private string treeTitle;
    private string iconOpened;
    private string iconClosed;

    private string assemblyName;
    private string type;
    private string action;

    public void Initialize( XmlNode xmlData ) {
      silent = bool.Parse( xmlData.Attributes[ "silent" ].Value );
      initialize = bool.Parse( xmlData.Attributes[ "initialize" ].Value );
      sortOrder = byte.Parse( xmlData.Attributes[ "sortOrder" ].Value );

      applicationAlias = xmlData.Attributes[ "applicationAlias" ].Value;
      treeAlias = xmlData.Attributes[ "treeAlias" ].Value;
      treeTitle = xmlData.Attributes[ "treeTitle" ].Value;
      iconOpened = xmlData.Attributes[ "iconOpened" ].Value;
      iconClosed = xmlData.Attributes[ "iconClosed" ].Value;

      assemblyName = xmlData.Attributes[ "assemblyName" ].Value;
      type = xmlData.Attributes[ "treeHandlerType" ].Value;
      action = xmlData.Attributes[ "action" ].Value;
    }

    #region IPackageAction Members

    public string Alias() {
      return "AddTree";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      if ( !umbraco.BusinessLogic.ApplicationTree.getAll().Any( t => t.Alias == treeAlias ) ) {
        umbraco.BusinessLogic.ApplicationTree.MakeNew( silent, initialize, sortOrder, applicationAlias, treeAlias, treeTitle, iconClosed, iconOpened, assemblyName, type, action );
      }
      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""addApplicationTree"" silent=""true/false""  initialize=""true/false"" sortOrder=""1"" applicationAlias=""appAlias"" treeAlias=""myTree"" treeTitle=""My Tree"" iconOpened=""folder_o.gif"" iconClosed=""folder.gif"" assemblyName=""umbraco"" treeHandlerType=""treeClass"" action=""alert('you clicked my tree')""/>", this.Alias() ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      umbraco.BusinessLogic.ApplicationTree.getByAlias( treeAlias ).Delete();
      return true;
    }

    #endregion
  }
}