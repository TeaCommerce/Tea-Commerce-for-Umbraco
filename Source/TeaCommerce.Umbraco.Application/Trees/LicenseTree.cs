using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Utils;
using Umbraco.Web.UI.Pages;
using umbraco.cms.presentation.Trees;
using System.Web;

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
        // Issue #86: If we return null during the GetSections request then it causes
        // backoffice errors due to umbraco not null checking so we only return null
        // if it's not part of the GetSections request
        if (!HttpContext.Current.Request.Url.AbsolutePath.EndsWith("/GetSections"))
        {
            rootNode = null;
        }
      }
    }

    public enum LicenseTreeNodeType {
      License
    }

  }
}