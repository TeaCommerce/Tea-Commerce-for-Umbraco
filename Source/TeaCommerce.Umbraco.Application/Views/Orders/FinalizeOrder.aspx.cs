using System;
using System.Security;
using System.Web;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Api.Web.PaymentProviders;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.Orders {
  public partial class FinalizeOrder : UmbracoProtectedPage {

    private Order order = OrderService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), new Guid( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, order.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      LitTransactionId.Text = CommonTerms.TransactionId;
      LitAmount.Text = CommonTerms.Amount;
      BtnFinalize.Text = PaymentProviderTerms.Finalize;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected void BtnFinalize_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        string transactionId = TxtTransactionId.Text;
        decimal? amount = TxtAmount.Text.ParseToDecimal();

        if ( amount != null && !string.IsNullOrEmpty( transactionId ) && order.PaymentInformation.PaymentMethodId != null ) {
          PaymentMethodService.Instance.Get( order.StoreId, order.PaymentInformation.PaymentMethodId.Value ).FinalizeOrder( order, amount.Value, transactionId );
          if ( order.IsFinalized ) {
            ClientTools.ChangeContentFrameUrl( WebUtils.GetPageUrl( Constants.Pages.EditOrder ) + "?storeId=" + order.StoreId + "&id=" + order.Id ).CloseModalWindow();
          }
        }


      }
    }

  }
}