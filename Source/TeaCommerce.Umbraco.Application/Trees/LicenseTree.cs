using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Utils;
using Umbraco.Web.UI.Pages;
using umbraco.cms.presentation.Trees;

namespace TeaCommerce.Umbraco.Application.Trees {
  public class LicenseTree : ABaseTree<LicenseTree.LicenseTreeNodeType> {

    public LicenseTree( string application )
      : base( application, LicenseTreeNodeType.License ) {
    }

    protected override void CreateRootNode( ref XmlTreeNode rootNode ) {
      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

      if ( permissions != null && PermissionService.Instance.GetCurrentLoggedInUserPermissions().HasPermission( GeneralPermissionType.AccessLicenses ) ) {
        AssignNodeValues( rootNode, GetNodeIdentifier( LicenseTreeNodeType.License ), DeveloperTerms.Licenses, Constants.TreeIcons.LicenseKey, "license" );
        rootNode.Source = null;
        rootNode.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.LicenseCheck ) ) + "})";
      } else {
        rootNode = null;
      }
    }

    public enum LicenseTreeNodeType {
      License
    }

  }
}