using System;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.ShippingMethods {
  public partial class CreateShippingMethod : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        ShippingMethod shippingMethod = new ShippingMethod( StoreId, TxtName.Text );
        shippingMethod.Save();

        Redirect( WebUtils.GetPageUrl( Constants.Pages.EditShippingMethod ) + "?id=" + shippingMethod.Id + "&storeId=" + shippingMethod.StoreId );
      }

    }

  }
}