using System;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.Countries {
  public partial class CreateCountry : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      LitDefaultCurrency.Text = CommonTerms.DefaultCurrency;
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
        Country country = new Country( StoreId, TxtName.Text, long.Parse( DrpCurrencies.SelectedValue ) );
        country.Save();

        Redirect( WebUtils.GetPageUrl( Constants.Pages.EditCountry ) + "?id=" + country.Id + "&storeId=" + country.StoreId );
      }

    }

  }
}