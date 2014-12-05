using System.Collections.Generic;
using System.Text;
using TeaCommerce.Api.Common;
using TeaCommerce.Umbraco.Application.Utils;
using umbraco.cms.presentation.Trees;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Application.Trees {
  public abstract class ABaseTree<T> : BaseTree where T : struct {

    private T defaultValue;

    public ABaseTree( string application, T defaultValue )
      : base( application ) {
      this.defaultValue = defaultValue;
    }

    protected override void CreateRootNodeActions( ref List<IAction> actions ) {
      actions.Clear();
    }

    protected override void CreateAllowedActions( ref List<IAction> actions ) {
      actions.Clear();
    }

    public override void Render( ref XmlTree tree ) {
    }

    public override void RenderJS( ref StringBuilder Javascript ) {

    }

    protected XmlTreeNode CreateNode( string nodeId, string text, string icon, string nodeType, bool hasChildNodes = false ) {
      return AssignNodeValues( XmlTreeNode.Create( this ), nodeId, text, icon, nodeType, hasChildNodes );
    }

    protected XmlTreeNode AssignNodeValues( XmlTreeNode node, string nodeId, string text, string icon, string nodeType, bool hasChildNodes = false ) {
      node.NodeID = nodeId;
      node.Text = text;
      node.Icon = WebUtils.GetWebResourceUrl( icon );
      node.NodeType = TreeAlias + "-" + nodeType;
      node.Source = hasChildNodes ? this.GetTreeServiceUrl( nodeId ) : null;
      node.Action = "javascript:(function(){})";
      return node;
    }

    protected static string GetNodeIdentifier( T type, params long[] list ) {
      return type.ToString() + ( list.Length > 0 ? "_" + string.Join( "_", list ) : "" );
    }

    protected T CurrentNodeType {
      get {
        if ( string.IsNullOrEmpty( NodeKey ) )
          return defaultValue;

        string[] strArray = NodeKey.Split( new char[] { '_' } );
        if ( strArray.Length == 0 )
          return defaultValue;

        return strArray[ 0 ].TryParse<T>() ?? defaultValue;
      }
    }

  }
}