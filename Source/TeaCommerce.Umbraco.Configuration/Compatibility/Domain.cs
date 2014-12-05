using System.Collections.Generic;
using System.Linq;
using TeaCommerce.Api.Infrastructure.Caching;
using umbraco.BusinessLogic;

namespace TeaCommerce.Umbraco.Configuration.Compatibility {
  public static class Domain {

    private const string CacheKey = "Domains";

    public static umbraco.cms.businesslogic.web.Domain[] GetDomainsById( int nodeId ) {
      List<umbraco.cms.businesslogic.web.Domain> domains = CacheService.Instance.GetCacheValue<List<umbraco.cms.businesslogic.web.Domain>>( CacheKey );

      if ( domains == null ) {
        domains = new List<umbraco.cms.businesslogic.web.Domain>();

        using ( var dr = Application.SqlHelper.ExecuteReader( "select id, domainName from umbracoDomains" ) ) {
          while ( dr.Read() ) {
            var domainName = dr.GetString( "domainName" );
            var domainId = dr.GetInt( "id" );
            if ( domains.All( d => d.Name != domainName ) ) {
              domains.Add( new umbraco.cms.businesslogic.web.Domain( domainId ) );
            }
          }
        }

        CacheService.Instance.SetCacheValue( CacheKey, domains );
      }

      return domains.Where( d => d.RootNodeId == nodeId ).ToArray();
    }

    public static void InvalidateCache() {
      CacheService.Instance.Invalidate( CacheKey );
    }
  }
}