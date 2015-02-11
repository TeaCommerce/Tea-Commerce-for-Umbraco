using System;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;
using TeaCommerce.Api.Common;

namespace TeaCommerce.Umbraco.Application.Views.GiftCards {
  public partial class CreateGiftCard : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );
      LitAmount.Text = CommonTerms.Amount;
      LitCurrency.Text = CommonTerms.Currency;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        DrpCurrencies.DataSource = CurrencyService.Instance.GetAll( StoreId );
        DrpCurrencies.DataBind();
      }
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        GiftCard giftCard = GiftCardService.Instance.Generate( StoreId, long.Parse( DrpCurrencies.SelectedValue ), TxtAmount.Text.ParseToDecimal() ?? 0M );

        Redirect( WebUtils.GetPageUrl( Constants.Pages.EditGiftCard ) + "?id=" + giftCard.Id + "&storeId=" + giftCard.StoreId, false );
      }
    }

  }
}