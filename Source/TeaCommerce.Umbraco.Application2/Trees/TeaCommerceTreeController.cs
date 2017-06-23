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
using System.Collections.Generic;
using System.Linq;

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
          treeNodes.Add( treeNode );
          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.ShippingMethods, currentStoreId ), id, queryStrings, "TODO: Shipping methods", "", true );
          treeNodes.Add( treeNode );
          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.PaymentMethods, currentStoreId ), id, queryStrings, "TODO: Payment methods", "", true );
          treeNodes.Add( treeNode );
          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Internationalization, currentStoreId ), id, queryStrings, "TODO: Internationalization", "", true );
          //node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsOrderStatuses, CurrentStoreId ), CommonTerms.OrderStatuses, Constants.TreeIcons.ClipboardTask, "settings-order-statuses", true );
          //node.Menu.Add( ActionNew.Instance );
          //node.Menu.Add( new SortOrderStatusesAction() );
          //node.Menu.Add( ContextMenuSeperator.Instance );
          //node.Menu.Add( ActionRefresh.Instance );
          treeNodes.Add( treeNode );
          break;
        case Constants.Trees.Internationalization:
          currentStoreId = GetCurrentStoreId( id );
          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Countries, currentStoreId ), id, queryStrings, "TODO: Countries", "", true );
          treeNodes.Add( treeNode );
          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Currencies, currentStoreId ), id, queryStrings, "TODO: Currencies", "", true );
          treeNodes.Add( treeNode );
          treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.VatGroups, currentStoreId ), id, queryStrings, "TODO: Vat Groups", "", true );
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
        case Constants.Trees.Countries:
          currentStoreId = GetCurrentStoreId( id );
          foreach ( Country country in CountryService.Instance.GetAll( 1 ) ) {//TODO: hardcoded id

            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Country, currentStoreId, country.Id ), id, queryStrings, country.Name, "", CountryRegionService.Instance.GetAll( currentStoreId, country.Id ).Any() );

            treeNode.RoutePath = "/teacommerce2/teacommerce2/country-edit/" + currentStoreId + "-" + country.Id;

            treeNodes.Add( treeNode );
          }
          break;
        case Constants.Trees.Country:
          currentStoreId = GetCurrentStoreId( id );
          foreach ( CountryRegion region in CountryRegionService.Instance.GetAll( 1, GetCurrentCountryId( id ) ) ) {
            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.CountryRegion, currentStoreId ), id, queryStrings, region.Name, "" );
            treeNode.RoutePath = "/teacommerce2/teacommerce2/countryRegion-edit/" + currentStoreId + "-" + region.Id;

            treeNodes.Add( treeNode );
          }
          break;
        case Constants.Trees.Currencies:
          currentStoreId = GetCurrentStoreId( id );
          foreach ( Currency currency in CurrencyService.Instance.GetAll( 1 ) ) {
            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.Currency, currentStoreId ), id, queryStrings, currency.Name, "" );
            treeNode.RoutePath = "/teacommerce2/teacommerce2/currency-edit/" + currentStoreId + "-" + currency.Id;

            treeNodes.Add( treeNode );
          }
          break;
        case Constants.Trees.ShippingMethods:
          currentStoreId = GetCurrentStoreId( id );
          foreach ( ShippingMethod shippingMethod in ShippingMethodService.Instance.GetAll( 1 ) ) {
            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.ShippingMethod, currentStoreId ), id, queryStrings, shippingMethod.Name, "" );
            treeNode.RoutePath = "/teacommerce2/teacommerce2/shippingMethod-edit/" + currentStoreId + "-" + shippingMethod.Id;

            treeNodes.Add( treeNode );
          }
          break;
        case Constants.Trees.PaymentMethods:
          currentStoreId = GetCurrentStoreId( id );
          foreach ( PaymentMethod paymentMethod in PaymentMethodService.Instance.GetAll( 1 ) ) {
            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.PaymentMethod, currentStoreId ), id, queryStrings, paymentMethod.Name, "" );
            treeNode.RoutePath = "/teacommerce2/teacommerce2/paymentMethod-edit/" + currentStoreId + "-" + paymentMethod.Id;

            treeNodes.Add( treeNode );
          }
          break;
        case Constants.Trees.VatGroups:
          currentStoreId = GetCurrentStoreId( id );
          foreach ( VatGroup vatGroups in VatGroupService.Instance.GetAll( 1 ) ) {
            treeNode = CreateTreeNode( GetNodeIdentifier( Constants.Trees.VatGroup, currentStoreId ), id, queryStrings, vatGroups.Name, "" );
            treeNode.RoutePath = "/teacommerce2/teacommerce2/vatGroup-edit/" + currentStoreId + "-" + vatGroups.Id;

            treeNodes.Add( treeNode );
          }
          break;
      }
      return treeNodes;
    }

    protected override MenuItemCollection GetMenuForNode( string id, FormDataCollection queryStrings ) {
      MenuItemCollection menuItems = new MenuItemCollection();
      MenuItem menuItem;

      switch ( GetCurrentNodeType( id ) ) {
        case Constants.Trees.OrderStatuses:
          menuItem = new MenuItem( ActionNew.Instance );
          menuItem.LaunchDialogView( "/App_Plugins/" + Constants.Applications.TeaCommerce + "/backoffice/" + Constants.Trees.TeaCommerce + "/order-status-create.html", "Create" ); //TODO: oversæt
          //menuItem.NavigateToRoute( "/" + Constants.Applications.TeaCommerce + "/" + Constants.Trees.TeaCommerce + "/store-edit/-1?create" );
          menuItems.Items.Add( menuItem );

          //TODO: sort
          menuItems.Items.Add<ActionSort>( "Sort" ); //TODO: oversæt
          menuItems.Items.Add<RefreshNode, ActionRefresh>( "Reload", true ); //TODO: oversæt
          break;
        case Constants.Trees.OrderStatus:
          menuItem = new MenuItem( ActionDelete.Instance );
          menuItem.LaunchDialogView( "/App_Plugins/" + Constants.Applications.TeaCommerce + "/backoffice/" + Constants.Trees.TeaCommerce + "/order-status-delete.html", "Delete"); //TODO: oversæt
          //menuItem.NavigateToRoute( "/" + Constants.Applications.TeaCommerce + "/" + Constants.Trees.TeaCommerce + "/store-edit/-1?create" );
          menuItems.Items.Add( menuItem );
          break;
      }

      return menuItems;
    }

    #region Helper methods

    private string GetNodeIdentifier( string type, params long[] list ) {
      return type + ( list.Length > 0 ? "-" + string.Join( "-", list ) : "" );
    }

    private long GetCurrentStoreId( string id ) {
      return long.Parse( id.Split( new[] { '-' } )[1] );
    }

    private long GetCurrentCountryId( string id ) {
      return long.Parse( id.Split( new[] { '-' } )[2] );
    }

    private string GetCurrentNodeType( string id ) {
      if ( id == "-1" )
        return id;

      string[] strArray = id.Split( new char[] { '-' } );
      if ( strArray.Length == 0 )
        return id;

      return strArray[0];
    }

    #endregion

  }
}
