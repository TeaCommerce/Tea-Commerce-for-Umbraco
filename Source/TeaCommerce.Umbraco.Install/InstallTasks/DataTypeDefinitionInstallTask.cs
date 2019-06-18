using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Install.InstallTasks
{
    public class DataTypeDefinitionInstallTask : ADbInstallTask
    {
        private readonly int _id;
        private readonly Guid _key;
        private readonly string _name;
        private readonly string _propertyEditorAlias;
        private readonly DataTypeDatabaseType _dbType;

        public DataTypeDefinitionInstallTask(int id, Guid key, string name, string propertyEditorAlias, DataTypeDatabaseType dbType)
        {
            _id = id;
            _key = key;
            _name = name;
            _propertyEditorAlias = propertyEditorAlias;
            _dbType = dbType;
        }

        public override void Install()
        {
            var dataTypeDefinitions = ApplicationContext.Current.Services.DataTypeService
                .GetDataTypeDefinitionByPropertyEditorAlias(_propertyEditorAlias);

            if (!dataTypeDefinitions.Any())
            {
                WithIdentityInsert("umbracoNode", () =>
                {
                    Database.Insert("umbracoNode", "id", false, new { id = _id, trashed = false, parentID = -1, nodeUser = 0, level = 0, path = "-1,"+ _id, sortOrder = 0, uniqueID = _key, text = _name, nodeObjectType = new Guid(Constants.ObjectTypes.DataType), createDate = DateTime.Now });
                });

                WithIdentityInsert("cmsDataType", () =>
                {
                    Database.Insert("cmsDataType", "pk", false, new { pk = _id, nodeId = _id, propertyEditorAlias = _propertyEditorAlias, dbType = _dbType.ToString() });
                });
            }
        }

        public override void Uninstall()
        {
            var dataTypeDefinitions = ApplicationContext.Current.Services.DataTypeService
                .GetDataTypeDefinitionByPropertyEditorAlias(_propertyEditorAlias);

            foreach (IDataTypeDefinition dataTypeDefinition in dataTypeDefinitions)
            {
                ApplicationContext.Current.Services.DataTypeService.Delete(dataTypeDefinition);
            }
        }
    }
}
