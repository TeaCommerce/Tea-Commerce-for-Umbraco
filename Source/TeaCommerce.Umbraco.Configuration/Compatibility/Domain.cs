using System.Collections.Generic;
using System.Linq;
using TeaCommerce.Api.Infrastructure.Caching;
using Umbraco.Core;

namespace TeaCommerce.Umbraco.Configuration.Compatibility {
  public static class Domain {

    private const string CacheKey = "Domains";

    public static umbraco.cms.businesslogic.web.Domain[] GetDomainsById( int nodeId ) {
      List<umbraco.cms.businesslogic.web.Domain> domains = CacheService.Instance.GetCacheValue<List<umbraco.cms.businesslogic.web.Domain>>( CacheKey );

      if ( domains == null ) {
        domains = new List<umbraco.cms.businesslogic.web.Domain>();

        List<dynamic> result = ApplicationContext.Current.DatabaseContext.Database.Fetch<dynamic>( "select id, domainName from umbracoDomains" );

        foreach ( dynamic domain in result ) {
          if ( domains.All( d => d.Name != domain.domainName ) ) {
            domains.Add( new umbraco.cms.businesslogic.web.Domain( domain.id ) );
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