using System;
using System.Linq;
using System.Web;
using Autofac;
using TeaCommerce.Api.Dependency;
using umbraco.cms.businesslogic.web;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public class LanguageService : ILanguageService {

    public static ILanguageService Instance { get { return DependencyContainer.Instance.Resolve<ILanguageService>(); } }

    public long? GetLanguageIdByNodePath( string nodePath ) {
      long? languageId = null;

      string[] pathNodeIds = nodePath.Split( new[] { "," }, StringSplitOptions.RemoveEmptyEntries );
      for ( int i = pathNodeIds.Length - 1; i > 0; i-- ) {
        Domain[] availableDomains = Compatibility.Domain.GetDomainsById( int.Parse( pathNodeIds[ i ] ) );

        if ( availableDomains != null && availableDomains.Length > 0 ) {

          //Try and find the domain and its related language (*1052 is the wildcard domain implemented in Umbraco 6.1)
          Domain domain = availableDomains.FirstOrDefault( d => d.Name.Equals( HttpContext.Current.Request.Url.Host ) || d.Name.Equals( "*" + pathNodeIds[ i ] ) );
          if ( domain != null ) {
            languageId = domain.Language.id;
            break;
          }

          //If no domain is found - then use the first available domains language - only in the first loop
          if ( languageId == null ) {
            languageId = availableDomains[ 0 ].Language.id;
          }
        }
      }

      return languageId;
    }

  }
}
