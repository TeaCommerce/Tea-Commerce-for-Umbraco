using System;
using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Marketing.Services;
using TeaCommerce.Api.Infrastructure.Security;
using umbraco.BusinessLogic.Actions;

namespace TeaCommerce.Umbraco.Application2.Trees {

  [PluginController( Constants.Applications.TeaCommerce )]
  [Tree( Constants.Applications.TeaCommerce, Constants.Trees.TeaCommerce, "Tea Commerce" )]
  public class TeaCommerceTreeController : TreeController {

    //protected override TreeNode CreateRootNode( FormDataCollection queryStrings ) {
    //  //TODO: root node can't be null
    //  TreeNode treeNode = base.CreateRootNode( queryStrings );
    //  treeNode.Name = Translations.TeaCommerce;
    //  treeNode.MenuUrl = "";
    //  return treeNode;
    //}

    protected override TreeNodeCollection GetTreeNodes( string id, FormDataCollection queryStrings ) {
      //TODO: sørg for at noder som ikke skal linke ikke laver 404
      TreeNodeCollection treeNodes = new TreeNodeCollection();
      TreeNode treeNode;

      switch ( GetCurrentNodeType( id ) ) {
        case "-1":
          //TODO: icons
          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Stores ), id, queryStrings, "TODO: Stores", "", true );
          treeNode.MenuUrl = "";
          treeNode.RoutePath = "";
          treeNodes.Add( treeNode );
          break;
        case Constants.Trees.Stores:
          Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

          foreach ( Store store in StoreService.Instance.GetAll() ) {
            if ( permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessStore, store.Id ) ) {
              treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Store, store.Id ), id, queryStrings, store.Name, "", true );
              treeNode.MenuUrl = "";
              treeNode.RoutePath = "";
              //treeNode = CreateNode( GetNodeIdentifier( StoreTreeNodeType.Store, store.Id, store.Id ), store.Name, Constants.TreeIcons.Store, "store", true );

              //if ( permissions.HasPermission( StoreSpecificPermissionType.AccessSettings, store.Id ) ) {
              //  treeNode.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditStore ) + "?id=" + store.Id ) + "})";
              //}

              //if ( permissions.HasPermission( GeneralPermissionType.CreateAndDeleteStore ) ) {
              //  treeNode.Menu.Add( ActionDelete.Instance );
              //}
              treeNodes.Add( treeNode );
            }
          }
          break;
        case Constants.Trees.Store:
          permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
          long currentStoreId = GetCurrentStoreId( id );

          if ( permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessSettings, 1 ) ) {//TODO: hardcoded id
            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Settings, currentStoreId ), id, queryStrings, "TODO: Settings", "", true );

            //node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.Settings, CurrentStoreId ), CommonTerms.Settings, Constants.TreeIcons.Toolbox, "settings", true );
            //node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditStore ) + "?id=" + CurrentStoreId ) + "})";
            //tree.Add( node );
            treeNodes.Add( treeNode );
          }
          break;
        case Constants.Trees.Settings:
          currentStoreId = GetCurrentStoreId( id );

          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.OrderStatuses, currentStoreId ), id, queryStrings, "TODO: Order statuses", "", true );
          //node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsOrderStatuses, CurrentStoreId ), CommonTerms.OrderStatuses, Constants.TreeIcons.ClipboardTask, "settings-order-statuses", true );
          //node.Menu.Add( ActionNew.Instance );
          //node.Menu.Add( new SortOrderStatusesAction() );
          //node.Menu.Add( ContextMenuSeperator.Instance );
          //node.Menu.Add( ActionRefresh.Instance );
          treeNodes.Add( treeNode );
          break;
        case Constants.Trees.OrderStatuses:
          currentStoreId = GetCurrentStoreId( id );

          foreach ( OrderStatus orderStatus in OrderStatusService.Instance.GetAll( 1 ) ) {//TODO: hardcoded id
            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.OrderStatus, currentStoreId ), id, queryStrings, orderStatus.Name, "" );
            treeNode.RoutePath = "/teacommerce2/teacommerce2/order-status-edit/" + currentStoreId + "-" + orderStatus.Id;
            //treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.OrderStatus, orderStatus.Id ), id, queryStrings, orderStatus.Name, "" );
            //treeNode.
            //node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsOrderStatus, CurrentStoreId, orderStatus.Id ), orderStatus.Name, Constants.TreeIcons.DocumentTask, "settings-order-status" );
            //node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditOrderStatus ) + "?id=" + orderStatus.Id + "&storeId=" + orderStatus.StoreId ) + "})";
            //node.Menu.Add( ActionDelete.Instance );
            treeNodes.Add( treeNode );
          }
          break;
      }
      return treeNodes;
    }

    protected override MenuItemCollection GetMenuForNode( string id, FormDataCollection queryStrings ) {
      MenuItemCollection menuItems = new MenuItemCollection();

      switch ( GetCurrentNodeType( id ) ) {
        case Constants.Trees.OrderStatuses:
          MenuItem menuItem = new MenuItem( ActionNew.Instance );
          menuItem.LaunchDialogView( "/App_Plugins/" + Constants.Applications.TeaCommerce + "/backoffice/" + Constants.Trees.TeaCommerce + "/order-status-create.html", "Create" ); //TODO: oversæt
          //menuItem.NavigateToRoute( "/" + Constants.Applications.TeaCommerce + "/" + Constants.Trees.TeaCommerce + "/store-edit/-1?create" );
          menuItems.Items.Add( menuItem );

          //TODO: sort
          menuItems.Items.Add<ActionSort>( "Sort" ); //TODO: oversæt
          menuItems.Items.Add<RefreshNode, ActionRefresh>( "Reload", true ); //TODO: oversæt
          break;
      }

      return menuItems;
    }

    #region Helper methods

    private string GetNodeIdentifier( string type, params long[] list ) {
      return type + ( list.Length > 0 ? "-" + string.Join( "-", list ) : "" );
    }

    private long GetCurrentStoreId( string id ) {
      return long.Parse( id.Split( new[] { '-' } )[ 1 ] );
    }

    private string GetCurrentNodeType( string id ) {
      if ( id == "-1" )
        return id;

      string[] strArray = id.Split( new char[] { '-' } );
      if ( strArray.Length == 0 )
        return id;

      return strArray[ 0 ];
    }

    #endregion

  }
}
