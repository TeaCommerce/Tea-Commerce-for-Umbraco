using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Utils;
using umbraco.BusinessLogic;
using umbraco.cms.presentation.Trees;
using Umbraco.Web.UI.Pages;

namespace TeaCommerce.Umbraco.Application.Trees {
  public class SecurityTree : ABaseTree<SecurityTree.SecurityTreeNodeType> {

    public SecurityTree( string application )
      : base( application, SecurityTreeNodeType.Security ) {
    }

    protected override void CreateRootNode( ref XmlTreeNode rootNode ) {
      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

      if ( permissions != null && permissions.HasPermission( GeneralPermissionType.AccessSecurity ) ) {
        AssignNodeValues( rootNode, GetNodeIdentifier( SecurityTreeNodeType.Security ), StoreTerms.Security, Constants.TreeIcons.Lock, "security", GetUsers().Any() );
      } else {
        rootNode = null;
      }
    }

    public override void Render( ref XmlTree tree ) {
      switch ( CurrentNodeType ) {
        case SecurityTreeNodeType.Security:
          foreach ( User user in GetUsers() ) {
            XmlTreeNode node = CreateNode( GetNodeIdentifier( SecurityTreeNodeType.User, user.Id ), user.Name, Constants.TreeIcons.User, "user" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditUserPermissions ) + "?id=" + user.Id ) + "})";
            tree.Add( node );
          }
          break;
      }
    }

    private IEnumerable<User> GetUsers() {
      List<User> users = new List<User>();

      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      User currentUser = umbraco.helper.GetCurrentUmbracoUser();

      //Show all users if current user is admin or has access to users section
      bool showAllUsers = currentUser != null && ( currentUser.IsRoot() || currentUser.Applications.Any( a => a.alias == "users" ) );

      foreach ( User user in User.getAll() ) {
        if ( user.IsRoot() || user.Applications.All( a => a.alias != "teacommerce" ) ) continue; //Don't ever show admin user

        bool showUser = showAllUsers || user.Id.ToString( CultureInfo.InvariantCulture ) == permissions.UserId;

        //If user still doesn't have access - then only show users that has access to the same store as current user
        if ( !showUser ) {
          Permissions userPermissions = PermissionService.Instance.Get( user.Id.ToString( CultureInfo.InvariantCulture ) );
          showUser = userPermissions != null && permissions.StoreSpecificPermissions.Any( p => p.Value.HasFlag( StoreSpecificPermissionType.AccessStore ) && userPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, p.Key ) );
        }

        if ( showUser ) {
          users.Add( user );
        }
      }

      return users;
    }

    public enum SecurityTreeNodeType {
      Security,
      User
    }

  }
}