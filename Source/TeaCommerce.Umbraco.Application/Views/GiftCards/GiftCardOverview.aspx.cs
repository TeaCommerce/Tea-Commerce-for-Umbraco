using System;
using System.Collections.Generic;
using System.Security;
using System.Web;
using TeaCommerce.Api.Infrastructure.Licensing;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Common;
using System.Linq;

namespace TeaCommerce.Umbraco.Application.Views.GiftCards {
  public partial class GiftCardOverview : UmbracoProtectedPage {

    protected readonly long StoreId = long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessMarketing, StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnlGiftCard );

      LitTrialMode.Text = CommonTerms.TrialModeMarketing;
      HypTrialMode.Text = CommonTerms.TrialModeBuy;
      PnSearchCriteria.Text = CommonTerms.SearchCriteria;
      PPnlCode.Text = CommonTerms.Code;
      PPnlOrderNumer.Text = CommonTerms.OrderNumer;
      PPnlCurrency.Text = CommonTerms.Currency;
      PPnlStartDate.Text = CommonTerms.StartDate;
      PPnlEndDate.Text = CommonTerms.EndDate;
      BtnSearch.Text = CommonTerms.Search;
      BtnReset.Text = CommonTerms.Reset;
      PnSearchResult.Text = CommonTerms.SearchResult;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        PnlLicenseCheck.Visible = !LicenseService.Instance.ValidateLicenseFeatures( Feature.Marketing );

        DrpCurrencies.DataSource = CurrencyService.Instance.GetAll( StoreId );
        DrpCurrencies.DataBind();

        TxtCode.Text = (string)Session[ "GiftCard_Search_Code" ];
        TxtOrderNumber.Text = (string)Session[ "GiftCard_Search_OrderNumber" ];
        DrpCurrencies.Items.TrySelectByValue( (long?)Session[ "GiftCard_Search_Currency" ] );
        DPStart.DateTime = (DateTime?)Session[ "GiftCard_Search_StartDate" ] ?? DateTime.MinValue;
        DPEnd.DateTime = (DateTime?)Session[ "GiftCard_Search_EndDate" ] ?? DateTime.MinValue;

        Search();
      }
    }

    protected void BtnSearch_Click( object sender, EventArgs e ) {
      Search();
    }

    protected void BtnReset_Click( object sender, EventArgs e ) {
      TxtCode.Text = "";
      TxtOrderNumber.Text = "";
      DrpCurrencies.SelectedValue = "";
      DPStart.DateTime = DateTime.MinValue;
      DPEnd.DateTime = DateTime.MinValue;

      Search();
    }

    private void Search() {
      string code = TxtCode.Text;
      string orderNumber = TxtOrderNumber.Text;
      long? currencyId = DrpCurrencies.SelectedValue.TryParse<long>();
      DateTime? startDate = DPStart.DateTime != DateTime.MinValue ? (DateTime?)DPStart.DateTime : null;
      DateTime? endDate = DPEnd.DateTime != DateTime.MinValue ? (DateTime?)DPEnd.DateTime : null;

      Session[ "GiftCard_Search_Code" ] = code;
      Session[ "GiftCard_Search_OrderNumber" ] = orderNumber;
      Session[ "GiftCard_Search_Currency" ] = currencyId;
      Session[ "GiftCard_Search_StartDate" ] = startDate;
      Session[ "GiftCard_Search_EndDate" ] = endDate;

      List<GiftCard> giftCards = GiftCardService.Instance.Search( StoreId, code, orderNumber, currencyId, startDate, endDate != null ? (DateTime?)endDate.Value.AddDays( 1 ) : null ).ToList();
      ICurrencyService currencyService = CurrencyService.Instance;
      List<Currency> currencies = giftCards.Select( gc => gc.CurrencyId ).Distinct().Select( c => currencyService.Get( StoreId, c ) ).ToList();

      LvGiftCards.DataSource = giftCards.Select( gc => new {
        gc.Id,
        gc.Code,
        gc.OriginalAmount,
        UsedAmount = gc.OriginalAmount - gc.RemainingAmount,
        gc.RemainingAmount,
        Currency = currencies.Single( c => c.Id == gc.CurrencyId ).IsoCode,
        DaysRemaining = ( gc.DateValidTo - DateTime.Today ).TotalDays
      } );
      LvGiftCards.DataBind();
      PnSearchResult.Visible = LvGiftCards.Items.Count > 0;
    }

  }
}