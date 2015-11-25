using System.Linq;
using System.Xml;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class AddTree : IPackageAction {

    private byte _sortOrder;
    private string _treeAlias;
    private string _treeTitle;
    private string _type;

    private void Initialize( XmlNode xmlData ) {
      if ( xmlData.Attributes == null ) return;
      _sortOrder = byte.Parse( xmlData.Attributes[ "sortOrder" ].Value );
      _treeAlias = xmlData.Attributes[ "treeAlias" ].Value;
      _treeTitle = xmlData.Attributes[ "treeTitle" ].Value;
      _type = xmlData.Attributes[ "type" ].Value;
    }

    #region IPackageAction Members

    public string Alias() {
      return "AddTree";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      if ( ApplicationContext.Current.Services.ApplicationTreeService.GetByAlias( _treeAlias ) == null ) {
        ApplicationContext.Current.Services.ApplicationTreeService.MakeNew( true, _sortOrder, "teacommerce", _treeAlias, _treeTitle, "folder_o.gif", "folder.gif", _type );
      }
      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( @"<Action runat=""install"" alias=""AddTree"" sortOrder=""1"" applicationAlias=""appAlias"" treeAlias=""myTree"" treeTitle=""My Tree"" type=""TeaCommerce.Umbraco.Application.Trees.StoreTree"" />" );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      ApplicationContext.Current.Services.ApplicationTreeService.DeleteTree( ApplicationContext.Current.Services.ApplicationTreeService.GetByAlias( _treeAlias ) );
      return true;
    }

    #endregion
  }
}