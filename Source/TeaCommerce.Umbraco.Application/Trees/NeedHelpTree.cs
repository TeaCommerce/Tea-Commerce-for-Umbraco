using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Utils;
using Umbraco.Web.UI.Pages;
using umbraco.cms.presentation.Trees;

namespace TeaCommerce.Umbraco.Application.Trees {
  public class NeedHelpTree : ABaseTree<NeedHelpTree.NeedHelpTreeNodeType> {

    public NeedHelpTree( string application )
      : base( application, NeedHelpTreeNodeType.NeedHelp ) {
    }

    protected override void CreateRootNode( ref XmlTreeNode rootNode ) {
      AssignNodeValues( rootNode, GetNodeIdentifier( NeedHelpTreeNodeType.NeedHelp ), DeveloperTerms.NeedHelp + "?", Constants.TreeIcons.Lifebuoy, "need-help" );
      rootNode.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.NeedHelp ) ) + "})";
    }

    public enum NeedHelpTreeNodeType {
      NeedHelp
    }



  }
}