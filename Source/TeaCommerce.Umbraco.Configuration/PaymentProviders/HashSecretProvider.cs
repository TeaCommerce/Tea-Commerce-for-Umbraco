using System;
using System.IO;
using System.Web.Hosting;
using TeaCommerce.Api.Infrastructure.Caching;
using TeaCommerce.Api.Web.PaymentProviders;

namespace TeaCommerce.Umbraco.Configuration.PaymentProviders {
  public class HashSecretProvider : IHashSecretProvider {

    private readonly ICacheService _cacheService;

    public HashSecretProvider( ICacheService cacheService ) {
      _cacheService = cacheService;
    }

    public string GetKey() {
      string key = _cacheService.GetCacheValue<string>( "HashSecret" ) ?? "";

      if ( string.IsNullOrEmpty( key ) ) {
        string hashSecretPath = HostingEnvironment.MapPath( "~/App_Data/TeaCommerce/hash-secret.txt" );

        if ( hashSecretPath != null ) {
          FileInfo file = new FileInfo( hashSecretPath );

          if ( !file.Exists ) {
            Random random = new Random();
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for ( int i = 0; i < 10; i++ ) {
              key += validChars[ random.Next( validChars.Length ) ];
            }

            if ( file.Directory != null ) {
              file.Directory.Create();
            }
            File.WriteAllText( file.FullName, key );
          } else {
            key = File.ReadAllText( file.FullName );
          }

          _cacheService.SetCacheValue( "HashSecret", key, TimeSpan.FromDays( 1d ) );
        }
      }

      return key;
    }
  }
}