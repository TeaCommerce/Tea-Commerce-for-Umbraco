using System;
using System.Linq;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Marketing.Services;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Trees.Actions;
using TeaCommerce.Umbraco.Application.Utils;
using umbraco.BusinessLogic.Actions;
using umbraco.cms.presentation.Trees;
using Umbraco.Web.UI.Pages;

namespace TeaCommerce.Umbraco.Application.Trees {
  public class StoreTree : ABaseTree<StoreTree.StoreTreeNodeType> {

    public StoreTree( string application )
      : base( application, StoreTreeNodeType.Stores ) {
    }

    protected override void CreateRootNode( ref XmlTreeNode rootNode ) {
      Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

      if ( permissions != null && ( permissions.StoreSpecificPermissions.Any( p => p.Value.HasFlag( StoreSpecificPermissionType.AccessStore ) ) || permissions.HasPermission( GeneralPermissionType.CreateAndDeleteStore ) ) ) {
        AssignNodeValues( rootNode, GetNodeIdentifier( StoreTreeNodeType.Stores ), CommonTerms.Stores, Constants.TreeIcons.Building, "stores", ( permissions.IsUserSuperAdmin && StoreService.Instance.GetAll().Any() ) || permissions.StoreSpecificPermissions.Any( p => p.Value.HasFlag( StoreSpecificPermissionType.AccessStore ) ) );

        if ( permissions.HasPermission( GeneralPermissionType.CreateAndDeleteStore ) ) {
          rootNode.Menu.Add( ActionNew.Instance );
          rootNode.Menu.Add( new SortStoresAction() );
          rootNode.Menu.Add( ContextMenuSeperator.Instance );
        }

        rootNode.Menu.Add( ActionRefresh.Instance );
      } else {
        rootNode = null;
      }
    }

    public override void Render( ref XmlTree tree ) {
      XmlTreeNode node;

      switch ( CurrentNodeType ) {
        case StoreTreeNodeType.Stores:
          #region Render tree
          Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

          foreach ( Store store in StoreService.Instance.GetAll() ) {
            if ( permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessStore, store.Id ) ) {
              node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.Store, store.Id, store.Id ), store.Name, Constants.TreeIcons.Store, "store", true );

              if ( permissions.HasPermission( StoreSpecificPermissionType.AccessSettings, store.Id ) ) {
                node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditStore ) + "?id=" + store.Id ) + "})";
              }

              if ( permissions.HasPermission( GeneralPermissionType.CreateAndDeleteStore ) ) {
                node.Menu.Add( ActionDelete.Instance );
              }
              tree.Add( node );
            }
          }
          #endregion
          break;
        case StoreTreeNodeType.Store:
          #region Render tree

          permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();

          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.Orders, CurrentStoreId ), CommonTerms.Orders, Constants.TreeIcons.Clipboard, "orders", true /*There is always a default order status*/ );
          tree.Add( node );

          if ( permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessMarketing, CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.Campaigns, CurrentStoreId ), CommonTerms.Marketing, Constants.TreeIcons.Target, "campaigns", CampaignService.Instance.GetAll( CurrentStoreId ).Any() );
            node.Menu.Add( ActionNew.Instance );
            node.Menu.Add( new SortCampaignsAction() );
            node.Menu.Add( ContextMenuSeperator.Instance );
            node.Menu.Add( ActionRefresh.Instance );
            tree.Add( node );

            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.GiftCards, CurrentStoreId ), CommonTerms.GiftCards, Constants.TreeIcons.Certificate, "giftCards" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.GiftCardOverview ) + "?storeId=" + CurrentStoreId ) + "})";
            node.Menu.Add( ActionNew.Instance );
            tree.Add( node );
          }

          if ( permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessSettings, CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.Settings, CurrentStoreId ), CommonTerms.Settings, Constants.TreeIcons.Toolbox, "settings", true );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditStore ) + "?id=" + CurrentStoreId ) + "})";
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.Orders:
          #region Render tree
          foreach ( OrderStatus orderStatus in OrderStatusService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.OrderStatus, CurrentStoreId, orderStatus.Id ), orderStatus.Name, Constants.TreeIcons.DocumentTask, "order-status" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.SearchOrders ) + "?storeId=" + orderStatus.StoreId + "&orderStatusId=" + orderStatus.Id ) + "})";
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.Campaigns:
          #region Render tree
          foreach ( Campaign campaign in CampaignService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.Campaign, CurrentStoreId, campaign.Id ), campaign.Name, Constants.TreeIcons.TagLabel, "campaign" );

            if ( !campaign.IsActive || ( campaign.StartDate != null && campaign.StartDate > DateTime.Now ) || ( campaign.EndDate != null && campaign.EndDate < DateTime.Now ) ) {
              node.Icon = WebUtils.GetWebResourceUrl( Constants.TreeIcons.TagLabelRed );
            }

            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditCampaign ) + "?id=" + campaign.Id + "&storeId=" + campaign.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.Settings:
          #region Render tree
          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsOrderStatuses, CurrentStoreId ), CommonTerms.OrderStatuses, Constants.TreeIcons.ClipboardTask, "settings-order-statuses", true );
          node.Menu.Add( ActionNew.Instance );
          node.Menu.Add( new SortOrderStatusesAction() );
          node.Menu.Add( ContextMenuSeperator.Instance );
          node.Menu.Add( ActionRefresh.Instance );
          tree.Add( node );


          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsShippingMethods, CurrentStoreId ), CommonTerms.ShippingMethods, Constants.TreeIcons.TruckBoxLabel, "settings-shipping-methods", ShippingMethodService.Instance.GetAll( CurrentStoreId ).Any() );
          node.Menu.Add( ActionNew.Instance );
          node.Menu.Add( new SortShippingMethodsAction() );
          node.Menu.Add( ContextMenuSeperator.Instance );
          node.Menu.Add( ActionRefresh.Instance );
          tree.Add( node );

          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsPaymentMethods, CurrentStoreId ), CommonTerms.PaymentMethods, Constants.TreeIcons.CreditCards, "settings-payment-methods", PaymentMethodService.Instance.GetAll( CurrentStoreId ).Any() );
          node.Menu.Add( ActionNew.Instance );
          node.Menu.Add( new SortPaymentMethodsAction() );
          node.Menu.Add( ContextMenuSeperator.Instance );
          node.Menu.Add( ActionRefresh.Instance );
          tree.Add( node );

          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsInternationalization, CurrentStoreId ), CommonTerms.Internationalization, Constants.TreeIcons.LocaleAlternate, "settings-internationalization", true );
          tree.Add( node );

          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsEmailTemplates, CurrentStoreId ), CommonTerms.EmailTemplates, Constants.TreeIcons.MailStack, "settings-email-templates", EmailTemplateService.Instance.GetAll( CurrentStoreId ).Any() );
          node.Menu.Add( ActionNew.Instance );
          node.Menu.Add( new SortEmailTemplatesAction() );
          node.Menu.Add( ContextMenuSeperator.Instance );
          node.Menu.Add( ActionRefresh.Instance );
          tree.Add( node );
          #endregion
          break;
        case StoreTreeNodeType.SettingsOrderStatuses:
          #region Render tree
          foreach ( OrderStatus orderStatus in OrderStatusService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsOrderStatus, CurrentStoreId, orderStatus.Id ), orderStatus.Name, Constants.TreeIcons.DocumentTask, "settings-order-status" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditOrderStatus ) + "?id=" + orderStatus.Id + "&storeId=" + orderStatus.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.SettingsShippingMethods:
          #region Render tree
          foreach ( ShippingMethod shippingMethod in ShippingMethodService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsShippingMethod, CurrentStoreId, shippingMethod.Id ), shippingMethod.Name, Constants.TreeIcons.BoxLabel, "settings-shipping-method" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditShippingMethod ) + "?id=" + shippingMethod.Id + "&storeId=" + shippingMethod.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.SettingsPaymentMethods:
          #region Render tree
          foreach ( PaymentMethod paymentMethod in PaymentMethodService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsPaymentMethod, CurrentStoreId, paymentMethod.Id ), paymentMethod.Name, Constants.TreeIcons.CreditCard, "settings-payment-method" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditPaymentMethod ) + "?id=" + paymentMethod.Id + "&storeId=" + paymentMethod.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.SettingsInternationalization:
          #region Render tree
          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsCountries, CurrentStoreId ), CommonTerms.Countries, Constants.TreeIcons.GlobeModel, "settings-countries", true /*There is always a default country*/ );
          node.Menu.Add( ActionNew.Instance );
          node.Menu.Add( new SortCountriesAction() );
          node.Menu.Add( ContextMenuSeperator.Instance );
          node.Menu.Add( ActionRefresh.Instance );
          tree.Add( node );

          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsCurrencies, CurrentStoreId ), CommonTerms.Currencies, Constants.TreeIcons.MoneyCoin, "settings-currencies", true /*There is always a default currency*/ );
          node.Menu.Add( ActionNew.Instance );
          node.Menu.Add( new SortCurrenciesAction() );
          node.Menu.Add( ContextMenuSeperator.Instance );
          node.Menu.Add( ActionRefresh.Instance );
          tree.Add( node );

          node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsVatGroups, CurrentStoreId ), CommonTerms.VatGroups, Constants.TreeIcons.ZoneMoney, "settings-vat-groups", true /*There is always a default vat group*/ );
          node.Menu.Add( ActionNew.Instance );
          node.Menu.Add( new SortVatGroupsAction() );
          node.Menu.Add( ContextMenuSeperator.Instance );
          node.Menu.Add( ActionRefresh.Instance );
          tree.Add( node );
          #endregion
          break;
        case StoreTreeNodeType.SettingsCountries:
          #region Render tree
          foreach ( Country country in CountryService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsCountry, CurrentStoreId, country.Id ), country.Name, Constants.TreeIcons.Map, "settings-country", CountryRegionService.Instance.GetAll( CurrentStoreId, country.Id ).Any() );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditCountry ) + "?id=" + country.Id + "&storeId=" + country.StoreId ) + "})";
            node.Menu.Add( ActionNew.Instance );
            node.Menu.Add( new SortCountryRegionsAction() );
            node.Menu.Add( ContextMenuSeperator.Instance );
            node.Menu.Add( ActionDelete.Instance );
            node.Menu.Add( ContextMenuSeperator.Instance );
            node.Menu.Add( ActionRefresh.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.SettingsCountry:
          #region Render tree
          long countryId = long.Parse( NodeKey.Split( new[] { '_' }, StringSplitOptions.RemoveEmptyEntries )[ 2 ] );
          foreach ( CountryRegion countryRegion in CountryRegionService.Instance.GetAll( CurrentStoreId, countryId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsCountryRegion, CurrentStoreId, countryRegion.Id ), countryRegion.Name, Constants.TreeIcons.Map, "settings-country-region" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditCountryRegion ) + "?id=" + countryRegion.Id + "&storeId=" + countryRegion.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.SettingsCurrencies:
          #region Render tree
          foreach ( Currency currency in CurrencyService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsCurrency, CurrentStoreId, currency.Id ), currency.Name, Constants.TreeIcons.Money, "settings-currency" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditCurrency ) + "?id=" + currency.Id + "&storeId=" + currency.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.SettingsVatGroups:
          #region Render tree
          foreach ( VatGroup vatGroup in VatGroupService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsVatGroup, CurrentStoreId, vatGroup.Id ), vatGroup.Name, Constants.TreeIcons.Zone, "settings-vat-group" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditVatGroup ) + "?id=" + vatGroup.Id + "&storeId=" + vatGroup.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;
        case StoreTreeNodeType.SettingsEmailTemplates:
          #region Render tree
          foreach ( EmailTemplate emailTemplate in EmailTemplateService.Instance.GetAll( CurrentStoreId ) ) {
            node = CreateNode( GetNodeIdentifier( StoreTreeNodeType.SettingsEmailTemplate, CurrentStoreId, emailTemplate.Id ), emailTemplate.Name, Constants.TreeIcons.Mail, "settings-email-template" );
            node.Action = "javascript:(function(){" + ClientTools.Scripts.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditEmailTemplate ) + "?id=" + emailTemplate.Id + "&storeId=" + emailTemplate.StoreId ) + "})";
            node.Menu.Add( ActionDelete.Instance );
            tree.Add( node );
          }
          #endregion
          break;

      }

    }

    protected long CurrentStoreId {
      get {
        return long.Parse( NodeKey.Split( new[] { '_' } )[ 1 ] );
      }
    }

    public enum StoreTreeNodeType {
      Stores,
      Store,
      Orders,
      OrderStatus,
      Campaigns,
      Campaign,
      GiftCards,
      Settings,
      SettingsOrderStatuses,
      SettingsOrderStatus,
      SettingsShippingMethods,
      SettingsShippingMethod,
      SettingsPaymentMethods,
      SettingsPaymentMethod,
      SettingsInternationalization,
      SettingsCountries,
      SettingsCountry,
      SettingsCountryRegion,
      SettingsCurrencies,
      SettingsCurrency,
      SettingsVatGroups,
      SettingsVatGroup,
      SettingsEmailTemplates,
      SettingsEmailTemplate
    }

  }
}