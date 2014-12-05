using System.Globalization;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Utils;
using Umbraco.Web.UI.Pages;
using umbraco.BusinessLogic;
using umbraco.cms.presentation.Trees;
using TeaCommerce.Api.Infrastructure.Security;
using System.Linq;

namespace TeaCommerce.Umbraco.Application.Trees {
  public class SecurityTree : ABaseTree<SecurityTree.SecurityTreeNodeType> {

    public SecurityTree( string application )
      : base( application, SecurityTreeNodeType.Security ) {
    }

    protected override void CreateRootNode( ref XmlTreeNode rootNode ) {
      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

      if ( permissions != null && permissions.HasPermission( GeneralPermissionType.AccessSecurity ) ) {
        AssignNodeValues( rootNode, GetNodeIdentifier( SecurityTreeNodeType.Security ), StoreTerms.Security, Constants.TreeIcons.Lock, "security", true );
      } else {
        rootNode = null;
      }
    }

    public override void Render( ref XmlTree tree ) {
      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

      switch ( CurrentNodeType ) {
        case SecurityTreeNodeType.Security:
          User currentUser = umbraco.helper.GetCurrentUmbracoUser();

          //Show all users if current user is admin or has access to users section
          bool showAllUsers = currentUser != null && ( currentUser.IsRoot() || currentUser.Applications.Any( a => a.alias == "users" ) );

          foreach ( User user in User.getAll() ) {

            if ( !user.IsRoot() ) { //Don't ever show admin user

              bool showUser = showAllUsers || user.Id.ToString( CultureInfo.InvariantCulture ) == permissions.UserId;

              //If user still doesn't have access - then only show users that has access to the same store as current user
              if ( !showUser ) {
                Permissions userPermissions = PermissionService.Instance.Get( user.Id.ToString( CultureInfo.InvariantCulture ) );
                showUser = userPermissions != null && permissions.StoreSpecificPermissions.Any( p => p.Value.HasFlag( StoreSpecificPermissionType.AccessStore ) && userPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, p.Key ) );
              }

              if ( showUser ) {
                XmlTreeNode node = CreateNode( GetNodeIdentifier( SecurityTreeNodeType.User, user.Id ), user.Name, Constants.TreeIcons.User, "user" );
                node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditUserPermissions ) + "?id=" + user.Id ) + "})";
                tree.Add( node );
              }
            }
          }
          break;
      }
    }

    public enum SecurityTreeNodeType {
      Security,
      User
    }

  }
}