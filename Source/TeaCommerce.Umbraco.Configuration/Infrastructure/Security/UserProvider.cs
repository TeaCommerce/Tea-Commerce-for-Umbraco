using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Security;
using umbraco.BusinessLogic;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Security {
  public class UserProvider : IUserProvider {

    public string GetCurrentLoggedInUserId() {
      string userId = "";

      User user = umbraco.helper.GetCurrentUmbracoUser();
      if ( user != null ) {
        userId = user.Id.ToString();
      }

      return userId;
    }

    public bool HasUserSuperAccess( string userId ) {
      userId.MustNotBeNullOrEmpty( "userId" );

      return userId == "0";
    }

  }
}
