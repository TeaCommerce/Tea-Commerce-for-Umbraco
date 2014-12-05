using System;
using System.Web.UI;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.OrderStatuses {
  public partial class CreateOrderStatus : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        OrderStatus orderStatus = new OrderStatus( StoreId, TxtName.Text );
        orderStatus.Save();
        base.Redirect( WebUtils.GetPageUrl( Constants.Pages.EditOrderStatus ) + "?id=" + orderStatus.Id + "&storeId=" + orderStatus.StoreId );
      }

    }

  }
}