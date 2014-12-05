using System.Web;
using TeaCommerce.Api.Persistence;

namespace TeaCommerce.Umbraco.Configuration.Persistence {
  public class DatabaseFactory : IDatabaseFactory {

    private readonly Database _database;

    public DatabaseFactory() {
      _database = CreateDatabase();
    }

    public Database Get() {
      Database database;

      if ( HttpContext.Current != null ) {
        database = (Database)HttpContext.Current.Items[ "CurrentDb" ];

        if ( database == null ) {
          HttpContext.Current.Items[ "CurrentDb" ] = database = CreateDatabase();
        }
      } else {
        database = _database;
      }

      return database;
    }

    private Database CreateDatabase() {
      return new Database( "umbracoDbDSN" );
    }
  }
}