using System.Collections.Generic;
using System.Xml;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.interfaces;
using System.Linq;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class CreateDataTypeDefinition : IPackageAction {

    private string _alias;
    private string _name;

    private void Initialize( XmlNode xmlData ) {
      _alias = xmlData.Attributes[ "dataTypeDefinitionAlias" ].Value;
      _name = xmlData.Attributes[ "name" ].Value;
    }

    #region IPackageAction Members

    public string Alias() {
      return "CreateDataTypeDefinition";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );

      IEnumerable<IDataTypeDefinition> dataTypeDefinitions = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByPropertyEditorAlias( _alias );
      if ( !dataTypeDefinitions.Any() ) {
        DataTypeDefinition dataTypeDefinition = new DataTypeDefinition( -1, _alias ) {
          Name = _name,
          DatabaseType = _alias != "TeaCommerce.StockManagement" ? DataTypeDatabaseType.Integer : DataTypeDatabaseType.Nvarchar
        };
        ApplicationContext.Current.Services.DataTypeService.Save( dataTypeDefinition );
      }

      return true;
    }

    public XmlNode SampleXml() {
      return null;
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      IEnumerable<IDataTypeDefinition> dataTypeDefinitions = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByPropertyEditorAlias( _alias );

      foreach ( IDataTypeDefinition dataTypeDefinition in dataTypeDefinitions ) {
        ApplicationContext.Current.Services.DataTypeService.Delete( dataTypeDefinition );
      }

      return true;
    }

    #endregion

  }
}