using System.Globalization;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Repositories;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Security {
  public class PermissionProvider : IPermissionProvider {

    private readonly IPermissionRepository _repository;

    public PermissionProvider( IPermissionRepository repository ) {
      _repository = repository;
    }

    public Permissions GetCurrentLoggedInUserPermissions() {
      return GetInternal();
    }

    public Permissions Get( string userId ) {
      userId.MustNotBeNullOrEmpty( "userId" );

      return GetInternal( userId );
    }

    private Permissions GetInternal( string userId = null ) {
      umbraco.BusinessLogic.User umbracoUser = null;

      if ( string.IsNullOrEmpty( userId ) ) {
        umbracoUser = umbraco.BusinessLogic.User.GetCurrent();
      } else {
        int? umbracoUserId = userId.TryParse<int>();
        if ( umbracoUserId != null ) {
          umbracoUser = umbraco.BusinessLogic.User.GetUser( umbracoUserId.Value );
        }
      }
      Permissions permissions = null;

      if ( umbracoUser != null ) {
        userId = umbracoUser.Id.ToString( CultureInfo.InvariantCulture );
        permissions = !umbracoUser.IsRoot() ? _repository.Get( userId ) : new Permissions( userId, true );
      }

      return permissions;
    }

  }
}
