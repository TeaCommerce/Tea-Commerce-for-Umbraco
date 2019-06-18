using System;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;

namespace TeaCommerce.Umbraco.Install.InstallTasks
{
    public abstract class AUmbracoDbInstallTask : IInstallTask
    {
        protected UmbracoDatabase Database => ApplicationContext.Current.DatabaseContext.Database;

        protected ISqlSyntaxProvider SqlSyntax = SqlSyntaxContext.SqlSyntaxProvider;

        public abstract void Install();

        public abstract void Uninstall();

        protected void WithIdentityInsert(string tableName, Action act)
        {
            if (SqlSyntax.SupportsIdentityInsert())
                Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} ON ", SqlSyntax.GetQuotedTableName(tableName))));

            act.Invoke();

            if (SqlSyntax.SupportsIdentityInsert())
                Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} OFF ", SqlSyntax.GetQuotedTableName(tableName))));
        }
    }
}
