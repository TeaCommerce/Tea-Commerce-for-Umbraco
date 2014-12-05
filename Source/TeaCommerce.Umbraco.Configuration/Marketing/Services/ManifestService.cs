using Autofac;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Umbraco.Configuration.Marketing.Models;
using Umbraco.Core.IO;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Services {
  public class ManifestService : IManifestService {

    public static IManifestService Instance { get { return DependencyContainer.Instance.Resolve<IManifestService>(); } }

    public IEnumerable<Manifest> GetAllForRules() {
      return GetAll( HostingEnvironment.MapPath( SystemDirectories.AppPlugins + "/TeaCommerce/Marketing/Rules" ) );
    }

    public IEnumerable<Manifest> GetAllForAwards() {
      return GetAll( HostingEnvironment.MapPath( SystemDirectories.AppPlugins + "/TeaCommerce/Marketing/Awards" ) );
    }

    private IEnumerable<Manifest> GetAll( string path ) {
      List<Manifest> manifests = new List<Manifest>();

      DirectoryInfo directory = new DirectoryInfo( path );
      if ( directory.Exists ) {
        foreach ( FileInfo file in directory.GetFiles( "*.manifest", SearchOption.AllDirectories ) ) {
          manifests.Add( JsonConvert.DeserializeObject<Manifest>( File.ReadAllText( file.FullName ) ) );
        }
      }

      return manifests;
    }
  }
}