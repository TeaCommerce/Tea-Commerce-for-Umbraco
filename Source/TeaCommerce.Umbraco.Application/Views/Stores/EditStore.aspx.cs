using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.Stores {
  public partial class EditStore : UmbracoProtectedPage {

    private Store store = StoreService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );
    private Permissions currentLoggedInUserPermissions;

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, store.Id ) ) {
        throw new SecurityException();
      }
      #endregion
      
      AddTab( CommonTerms.Common, PnCommon, SaveButton_Clicked );
      AddTab( StoreTerms.Order, PnOrder, SaveButton_Clicked );
      AddTab( StoreTerms.Product, PnProduct, SaveButton_Clicked );
      AddTab( CommonTerms.GiftCards, PnGiftCard, SaveButton_Clicked );
      AddTab( "UI", PnTemplateRendering, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlDefaultCountry.Text = StoreTerms.DefaultCountry;
      PPnlDefaultVatGroup.Text = StoreTerms.DefaultVatGroup;
      PPnlDefaultOrderStatus.Text = StoreTerms.DefaultOrderStatus;
      PPnlConfirmationEmail.Text = StoreTerms.ConfirmationEmailTemplate;
      PPnlPaymentInconsistencyEmail.Text = StoreTerms.PaymentInconsistencyEmailTemplate;
      PPnlPricesIsSpecifiedWithVat.Text = StoreTerms.PricesIsSpecifiedWithVat;
      PPnlChkPersistOrderId.Text = StoreTerms.UseCookies;
      PPnlChkOrderPersistanceTimeout.Text = StoreTerms.CookieTimeout + "<br /><small>" + StoreTerms.CookieTimeoutHelp + "</small>";

      PPnlCartNumberPrefix.Text = StoreTerms.CartNumberPrefix;
      PPnlOrderNumberPrefix.Text = StoreTerms.OrderNumberPrefix;

      PPnlProductPropertyAliases.Text = StoreTerms.ProductPropertyAliases;
      PPnlProductUniquenessPropertyAliases.Text = StoreTerms.ProductUniquenessPropertyAliases;
      PPnlProductVariantPropertyAlias.Text = StoreTerms.ProductVariantPropertyAlias;
      PPnlStockSharingStore.Text = StoreTerms.StockSharingStore + "<br /><small>" + StoreTerms.StockSharingStoreHelp + "</small>";

      PPnlGiftCardLength.Text = StoreTerms.Length;
      PPnlGiftCardDaysValid.Text = StoreTerms.DaysValid;
      PPnlGiftCardPrefix.Text = StoreTerms.Prefix;
      PPnlGiftCardSuffix.Text = StoreTerms.Suffix;

      PPnlEditOrderUIFile.Text = StoreTerms.EditOrderUiFile;
      PPnlTemplateRendering.Text = StoreTerms.AllowedTemplateFilesForRendering + "<br /><small>" + StoreTerms.AllowedTemplateFilesForRenderingHelp + "</small>";
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {

        TemplateFileSelectorControl.LoadTemplateFiles();
        TemplateFileSelectionListControl.LoadTemplateFiles();

        TxtName.Text = store.Name;
        DrpCountries.DataSource = CountryService.Instance.GetAll( store.Id );
        DrpCountries.DataBind();
        DrpCountries.SelectedValue = store.GeneralSettings.DefaultCountryId.ToString();

        DrpVatGroups.DataSource = VatGroupService.Instance.GetAll( store.Id );
        DrpVatGroups.DataBind();
        DrpVatGroups.SelectedValue = store.GeneralSettings.DefaultVatGroupId.ToString();

        DrpOrderStatuses.DataSource = OrderStatusService.Instance.GetAll( store.Id );
        DrpOrderStatuses.DataBind();
        DrpOrderStatuses.SelectedValue = store.GeneralSettings.DefaultOrderStatusId.ToString();

        DrpConfirmationEmail.DataSource = EmailTemplateService.Instance.GetAll( store.Id );
        DrpConfirmationEmail.DataBind();
        DrpConfirmationEmail.Items.TrySelectByValue( store.GeneralSettings.ConfirmationEmailTemplateId );

        DrpPaymentInconsistencyEmail.DataSource = EmailTemplateService.Instance.GetAll( store.Id );
        DrpPaymentInconsistencyEmail.DataBind();
        DrpPaymentInconsistencyEmail.Items.TrySelectByValue( store.GeneralSettings.PaymentInconsistencyEmailTemplateId );

        ChkPricesIsSpecifiedWithVat.Checked = store.GeneralSettings.PricesIsSpecifiedWithVat;

        ChkPersistOrderId.Checked = store.GeneralSettings.CookieTimeout != null;
        if ( store.GeneralSettings.CookieTimeout != null ) {
          TxtOrderPersistanceTimeout.Text = store.GeneralSettings.CookieTimeout.Value.TotalMinutes.ToString( "0" );
        }

        TxtCartNumberPrefix.Text = store.OrderSettings.CartNumberPrefix;
        TxtOrderNumberPrefix.Text = store.OrderSettings.OrderNumberPrefix;

        TxtProductPropertyAliases.Text = string.Join( ",", store.ProductSettings.ProductPropertyAliases );
        TxtProductUniquenessPropertyAliases.Text = string.Join( ",", store.ProductSettings.ProductUniquenessPropertyAliases );
        TxtProductVariantPropertyAlias.Text = store.ProductSettings.ProductVariantPropertyAlias;

        IEnumerable<Store> stores = StoreService.Instance.GetAll().ToList();
        IEnumerable<Store> stockSharingStores = stores.Where( s => s.ProductSettings.StockSharingStoreId == store.Id ).ToList();
        PPnlStockSharingStore.Visible = stores.Count() > 1;
        DrpStockSharingStore.Visible = !stockSharingStores.Any();
        LblStockSharingStoreAssigned.Visible = !DrpStockSharingStore.Visible;

        if ( !stockSharingStores.Any() ) {
          foreach ( Store store1 in stores ) {
            if ( store1.Id != store.Id ) {
              bool hasAccessToStore = currentLoggedInUserPermissions != null && currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, store1.Id );

              if ( hasAccessToStore || store1.Id == store.ProductSettings.StockSharingStoreId ) {
                DrpStockSharingStore.Items.Add( new ListItem( ( !hasAccessToStore ? "* " : "" ) + store1.Name, store1.Id.ToString( CultureInfo.InvariantCulture ) ) );
              }
            }
          }
          DrpStockSharingStore.Items.TrySelectByValue( store.ProductSettings.StockSharingStoreId );
        } else {
          LblStockSharingStoreAssigned.Text = string.Join( ", ", stockSharingStores.Select( s => s.Name ) );
        }

        TxtGiftCardLength.Text = store.GiftCardSettings.Length.ToString( CultureInfo.InvariantCulture );
        TxtGiftCardDaysValid.Text = store.GiftCardSettings.DaysValid.ToString( CultureInfo.InvariantCulture );
        TxtGiftCardPrefix.Text = store.GiftCardSettings.Prefix;
        TxtGiftCardSuffix.Text = store.GiftCardSettings.Suffix;

        TemplateFileSelectorControl.Items.TrySelectByValue( store.UISettings.EditOrderUiFile );
        TemplateFileSelectionListControl.Items.Cast<ListItem>().ToList().ForEach( i => i.Selected = store.UISettings.AllowedFilesForClientRendering.Contains( i.Value ) );
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {

        store.Name = TxtName.Text;
        store.GeneralSettings.DefaultCountryId = long.Parse( DrpCountries.SelectedValue );
        store.GeneralSettings.DefaultVatGroupId = long.Parse( DrpVatGroups.SelectedValue );
        store.GeneralSettings.DefaultOrderStatusId = long.Parse( DrpOrderStatuses.SelectedValue );
        store.GeneralSettings.ConfirmationEmailTemplateId = DrpConfirmationEmail.Text.TryParse<long>();
        store.GeneralSettings.PaymentInconsistencyEmailTemplateId = DrpPaymentInconsistencyEmail.Text.TryParse<long>();
        store.GeneralSettings.PricesIsSpecifiedWithVat = ChkPricesIsSpecifiedWithVat.Checked;
        store.GeneralSettings.CookieTimeout = ChkPersistOrderId.Checked ? (TimeSpan?)TimeSpan.FromMinutes( TxtOrderPersistanceTimeout.Text.TryParse<int>() ?? 0 ) : null;

        store.OrderSettings.CartNumberPrefix = TxtCartNumberPrefix.Text;
        store.OrderSettings.OrderNumberPrefix = TxtOrderNumberPrefix.Text;

        store.ProductSettings.ProductPropertyAliases = TxtProductPropertyAliases.Text.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).Select( i => i.Trim() ).ToList();
        store.ProductSettings.ProductUniquenessPropertyAliases = TxtProductUniquenessPropertyAliases.Text.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).Select( i => i.Trim() ).ToList();
        store.ProductSettings.ProductVariantPropertyAlias = TxtProductVariantPropertyAlias.Text;

        store.ProductSettings.StockSharingStoreId = DrpStockSharingStore.SelectedValue.TryParse<long>();
        if ( store.ProductSettings.StockSharingStoreId != null && ( store.ProductSettings.StockSharingStoreId == store.Id || currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, store.ProductSettings.StockSharingStoreId.Value ) ) ) {
          store.ProductSettings.StockSharingStoreId = null;
        }

        store.GiftCardSettings.Length = TxtGiftCardLength.Text.TryParse<int>() ?? 0;
        store.GiftCardSettings.DaysValid = TxtGiftCardDaysValid.Text.TryParse<int>() ?? 0;
        store.GiftCardSettings.Prefix = TxtGiftCardPrefix.Text;
        store.GiftCardSettings.Suffix = TxtGiftCardSuffix.Text;

        store.UISettings.EditOrderUiFile = TemplateFileSelectorControl.SelectedValue;
        store.UISettings.AllowedFilesForClientRendering = TemplateFileSelectionListControl.Items.Cast<ListItem>().Where( i => i.Selected ).Select( i => i.Value ).ToList();

        store.Save();

        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, StoreTerms.StoreSaved, string.Empty );
      }
    }

  }
}