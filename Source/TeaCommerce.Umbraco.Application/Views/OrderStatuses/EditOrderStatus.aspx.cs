using System;
using System.Security;
using System.Web;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.OrderStatuses {
  public partial class EditOrderStatus : UmbracoProtectedPage {

    private OrderStatus orderStatus = OrderStatusService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, orderStatus.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion
      
      AddTab( CommonTerms.Common, PnCommon, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlDictionaryItemName.Text = CommonTerms.Alias;
      PPnlRecalculateFinalizedOrder.Text = CommonTerms.RecalculateFinalizedOrder;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );
      
      if ( !IsPostBack ) {
        TxtName.Text = orderStatus.Name;
        TxtDictionaryItemName.Text = orderStatus.Alias;
        ChkRecalculateFinalizedOrder.Checked = orderStatus.RecalculateFinalizedOrder;
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        orderStatus.Name = TxtName.Text;
        orderStatus.Alias = TxtDictionaryItemName.Text.Trim();
        orderStatus.RecalculateFinalizedOrder = ChkRecalculateFinalizedOrder.Checked;
        orderStatus.Save();
        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.OrderStatusSaved, string.Empty );
      }
    }

  }
}