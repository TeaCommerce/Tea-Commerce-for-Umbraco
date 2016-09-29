using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class DataTypeDefinitionInstallTask : IInstallTask {

    private readonly string _name;
    private readonly string _propertyEditorAlias;

    public DataTypeDefinitionInstallTask(string name, string propertyEditorAlias) {
      _name = name;
      _propertyEditorAlias = propertyEditorAlias;
    }

    public void Install() {
      IEnumerable<IDataTypeDefinition> dataTypeDefinitions = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByPropertyEditorAlias( _propertyEditorAlias );
      if ( !dataTypeDefinitions.Any() ) {
        DataTypeDefinition dataTypeDefinition = new DataTypeDefinition( -1, _propertyEditorAlias ) {
          Name = _name,
          DatabaseType = _propertyEditorAlias != "TeaCommerce.StockManagement" ? DataTypeDatabaseType.Integer : DataTypeDatabaseType.Nvarchar
        };
        ApplicationContext.Current.Services.DataTypeService.Save( dataTypeDefinition );
      }
    }

    public void Uninstall() {
      IEnumerable<IDataTypeDefinition> dataTypeDefinitions = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByPropertyEditorAlias( _propertyEditorAlias );

      foreach ( IDataTypeDefinition dataTypeDefinition in dataTypeDefinitions ) {
        ApplicationContext.Current.Services.DataTypeService.Delete( dataTypeDefinition );
      }
    }
  }
}
