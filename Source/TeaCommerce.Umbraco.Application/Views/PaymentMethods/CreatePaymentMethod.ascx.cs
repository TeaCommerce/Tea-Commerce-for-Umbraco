using System;
using System.Web.UI;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.PaymentMethods {
  public partial class CreatePaymentMethod : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        PaymentMethod paymentMethod = new PaymentMethod( StoreId, TxtName.Text );
        paymentMethod.Save();
        base.Redirect( WebUtils.GetPageUrl( Constants.Pages.EditPaymentMethod ) + "?id=" + paymentMethod.Id + "&storeId=" + paymentMethod.StoreId );
      }

    }

  }
}