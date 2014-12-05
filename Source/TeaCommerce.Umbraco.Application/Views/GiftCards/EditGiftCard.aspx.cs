using System;
using System.Globalization;
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

namespace TeaCommerce.Umbraco.Application.Views.GiftCards {
  public partial class EditGiftCard : UmbracoProtectedPage {

    private GiftCard _giftCard = GiftCardService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessMarketing, _giftCard.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnlGiftCard, BtnSave_Click );

      PPnlCode.Text = CommonTerms.Code;
      PPnlOriginalAmount.Text = CommonTerms.OriginalAmount;
      PPnlRemainingAmount.Text = CommonTerms.RemainingAmount;
      PPnlCurrency.Text = CommonTerms.Currency;
      PPnlValidTo.Text = CommonTerms.ValidTo;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        //Load currencies
        ICurrencyService currencyService = CurrencyService.Instance;
        DrpCurrencies.DataSource = currencyService.GetAll( _giftCard.StoreId );
        DrpCurrencies.DataBind();

        LitCode.Text = _giftCard.Code;
        TxtOriginalAmount.Text = _giftCard.OriginalAmount.ToString( "0.####" );
        TxtRemainingAmount.Text = _giftCard.RemainingAmount.ToString( "0.####" );
        if ( !DrpCurrencies.Items.TrySelectByValue( _giftCard.CurrencyId ) ) {
          Currency currency = currencyService.Get( _giftCard.StoreId, _giftCard.CurrencyId );
          if ( currency != null ) {
            DrpCurrencies.Items.Add( new ListItem( "* " + currency.Name, currency.Id.ToString( CultureInfo.InvariantCulture ) ) );
            DrpCurrencies.Items.TrySelectByValue( _giftCard.CurrencyId );
          }
        }
        DPValidTo.DateTime = _giftCard.DateValidTo;
      }
    }

    protected void BtnSave_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        _giftCard.OriginalAmount = TxtOriginalAmount.Text.ParseToDecimal() ?? 0M;
        _giftCard.RemainingAmount = TxtRemainingAmount.Text.ParseToDecimal() ?? 0M;
        _giftCard.CurrencyId = long.Parse( DrpCurrencies.SelectedValue );
        _giftCard.DateValidTo = DPValidTo.DateTime != DateTime.MinValue ? DPValidTo.DateTime : DateTime.MaxValue;

        _giftCard.Save();

        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.GiftCardSaved, string.Empty );
      }

    }

  }
}