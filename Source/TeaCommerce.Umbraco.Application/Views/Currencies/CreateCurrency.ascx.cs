using System;
using System.Web.UI;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.Currencies {
  public partial class CreateCurrency : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      LitCultureName.Text = CommonTerms.CultureName;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        CultureCodeSelectorControl.LoadCultureCodes();
      }
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        Currency currency = new Currency( StoreId, TxtName.Text, CultureCodeSelectorControl.SelectedValue );
        currency.Save();
        base.Redirect( WebUtils.GetPageUrl( Constants.Pages.EditCurrency ) + "?id=" + currency.Id + "&storeId=" + currency.StoreId );
      }

    }

  }
}