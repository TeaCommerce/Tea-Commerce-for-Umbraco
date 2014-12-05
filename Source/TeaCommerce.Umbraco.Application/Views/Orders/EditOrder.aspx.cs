using System;
using System.Globalization;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Infrastructure.Templating;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Api.Web.PaymentProviders;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;
using umbraco.uicontrols;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.Orders {
  public partial class EditOrder : UmbracoProtectedPage {

    private readonly Order _order = OrderService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), new Guid( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, _order.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      TabPage tabPageCommon = AddTab( CommonTerms.Common, PnlOrder, BtnSave_Click );
      AddTab( CommonTerms.OrderStatus, PnOrderStatus, BtnSave_Click );
      AddTab( CommonTerms.Email, PnEmail );
      AddTab( CommonTerms.Transaction, PnTransaction );

      AddButton( "btn btn-danger btnDelete", CommonTerms.Delete, tabPageCommon, BtnDelete_Click );

      MenuButton btnPrinter = tabPageCommon.Menu.NewButton();
      btnPrinter.CssClass = "btn btn-default";
      btnPrinter.Text = CommonTerms.Print;
      btnPrinter.OnClientClick = "window.print();return false;";

      PPnlOrderStatus.Text = CommonTerms.OrderStatus;

      PPnlSendEmail.Text = BtnSendEmail.Text = CommonTerms.SendEmail;

      PPnlTransactionPaymentName.Text = CommonTerms.PaymentMethod;
      PPnlTransactionAmount.Text = CommonTerms.Amount;
      PPnlTransactionStatus.Text = CommonTerms.PaymentState;
      LblInconsistentPayment.Text = " (" + CommonTerms.InconsistentPayment + ")";
      PPnlTransactionId.Text = CommonTerms.TransactionId;
      PPnlCartName.Text = CommonTerms.ReferenceId;
      PPnlError.Text = CommonTerms.Error;
      LblError.Text = PaymentProviderTerms.PaymentProviderError;
      BtnFinalize.Text = PaymentProviderTerms.Finalize;
      BtnCancel.Text = PaymentProviderTerms.CancelPayment;
      BtnCapture.Text = PaymentProviderTerms.CapturePayment;
      BtnApprove.Text = PaymentProviderTerms.ApproveOrder;
      BtnRefund.Text = PaymentProviderTerms.RefundPayment;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        OrderStatusSelectorControl.LoadOrderStatuses( _order.StoreId );
        LoadEmails();

        if ( !OrderStatusSelectorControl.Items.TrySelectByValue( _order.OrderStatusId ) ) {
          OrderStatus orderStatus = OrderStatusService.Instance.Get( _order.StoreId, _order.OrderStatusId );
          if ( orderStatus != null ) {
            OrderStatusSelectorControl.Items.Add( new ListItem( "* " + orderStatus.Name, orderStatus.Id.ToString( CultureInfo.InvariantCulture ) ) );
            OrderStatusSelectorControl.Items.TrySelectByValue( _order.OrderStatusId );
          }
        }

        PaymentMethod paymentMethod = null;
        if ( _order.PaymentInformation.PaymentMethodId != null ) {
          paymentMethod = PaymentMethodService.Instance.Get( _order.StoreId, _order.PaymentInformation.PaymentMethodId.Value );
          paymentMethod.GetStatus( _order );
          LblTransactionPaymentName.Text = paymentMethod.Name;
          BtnFinalize.Visible = !_order.IsFinalized;
        }
        LblCartName.Text = _order.CartNumber;
        SetTransactionPayment( paymentMethod );
      }
    }

    protected override void OnPreRender( EventArgs e ) {
      LitXSLTContent.Text = TemplatingService.Instance.RenderTemplateFile( StoreService.Instance.Get( _order.StoreId ).UISettings.EditOrderUiFile, _order );
      base.OnPreRender( e );
    }

    protected void BtnSave_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        long orderStatusId = long.Parse( OrderStatusSelectorControl.SelectedValue );
        if ( orderStatusId != _order.OrderStatusId ) {
          _order.OrderStatusId = orderStatusId;
          _order.Save();
        }

        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.OrderSaved, string.Empty );
      }
    }

    protected void BtnDelete_Click( object sender, EventArgs e ) {
      if ( _order.Delete() ) {
        Response.Redirect( WebUtils.GetPageUrl( Constants.Pages.SearchOrders ) + "?storeId=" + _order.StoreId + "&orderStatusId=" + _order.OrderStatusId );
      }
    }

    protected void BtnFinalize_Click( object sender, EventArgs e ) {
      if ( !_order.IsFinalized ) {
        ClientTools.OpenModalWindow( WebUtils.GetPageUrl( Constants.Pages.FinalizeOrder ) + "?storeId=" + _order.StoreId + "&id=" + _order.Id, PaymentProviderTerms.Finalize, true, 420, 270, 0, 0, string.Empty, string.Empty );
      }
    }

    protected void BtnCancel_Click( object sender, EventArgs e ) {
      if ( _order.PaymentInformation.PaymentMethodId != null ) {
        PaymentMethod paymentMethod = PaymentMethodService.Instance.Get( _order.StoreId, _order.PaymentInformation.PaymentMethodId.Value );
        paymentMethod.CancelPayment( _order );
        SetTransactionPayment( paymentMethod );
      }
    }

    protected void BtnCapture_Click( object sender, EventArgs e ) {
      if ( _order.PaymentInformation.PaymentMethodId != null ) {
        PaymentMethod paymentMethod = PaymentMethodService.Instance.Get( _order.StoreId, _order.PaymentInformation.PaymentMethodId.Value );
        paymentMethod.CapturePayment( _order );
        SetTransactionPayment( paymentMethod );
      }
    }

    protected void BtnApprove_Click( object sender, EventArgs e ) {
      if ( _order.PaymentInformation.PaymentMethodId != null ) {
        PaymentMethod paymentMethod = PaymentMethodService.Instance.Get( _order.StoreId, _order.PaymentInformation.PaymentMethodId.Value );
        _order.TransactionInformation.InconsistentPayment = false;
        _order.Save();
        SetTransactionPayment( paymentMethod );
      }
    }

    protected void BtnRefund_Click( object sender, EventArgs e ) {
      if ( _order.PaymentInformation.PaymentMethodId != null ) {
        PaymentMethod paymentMethod = PaymentMethodService.Instance.Get( _order.StoreId, _order.PaymentInformation.PaymentMethodId.Value );
        paymentMethod.RefundPayment( _order );
        SetTransactionPayment( paymentMethod );
      }
    }

    private void SetTransactionPayment( PaymentMethod paymentMethod ) {
      if ( _order.TransactionInformation.PaymentState != null ) {
        LblTransactionStatus.Text = CommonTerms.ResourceManager.GetString( _order.TransactionInformation.PaymentState.Value.ToString() );
      }
      LblInconsistentPayment.Visible = _order.TransactionInformation.InconsistentPayment;
      LblTransactionAmount.Text = _order.TransactionInformation.AmountAuthorized.Formatted;
      LblTransactionId.Text = _order.TransactionInformation.TransactionId;
      if ( paymentMethod != null ) {
        IPaymentProvider paymentProvider = PaymentProviderService.Instance.Get( paymentMethod.PaymentProviderAlias );

        BtnCancel.Visible = _order.TransactionInformation.PaymentState == PaymentState.Authorized && paymentProvider.SupportsCancellationOfPayment && paymentMethod.AllowsCancellationOfPayment;
        BtnCapture.Visible = _order.TransactionInformation.PaymentState == PaymentState.Authorized && paymentProvider.SupportsCapturingOfPayment && paymentMethod.AllowsCapturingOfPayment && !_order.TransactionInformation.InconsistentPayment;
        BtnApprove.Visible = _order.TransactionInformation.InconsistentPayment;
        BtnRefund.Visible = _order.TransactionInformation.PaymentState == PaymentState.Captured && paymentProvider.SupportsRefundOfPayment && paymentMethod.AllowsRefundOfPayment;
      }
    }

    protected void BtnSendEmail_Click( object sender, EventArgs e ) {
      long? emailTemplateId = DrpEmailTemplates.SelectedValue.TryParse<long>();

      if ( emailTemplateId != null ) {
        EmailTemplate emailTemplate = EmailTemplateService.Instance.Get( _order.StoreId, emailTemplateId.Value );
        if ( emailTemplate != null ) {
          LblEmailSent.Text = emailTemplate.Send( _order ) ? CommonTerms.EmailSent : CommonTerms.EmailError;
          LblEmailSent.Visible = true;
        }
      }
    }

    private void LoadEmails() {
      DrpEmailTemplates.Items.Clear();
      DrpEmailTemplates.Items.Add( new ListItem( "----", string.Empty ) );

      DrpEmailTemplates.DataSource = EmailTemplateService.Instance.GetAll( _order.StoreId );
      DrpEmailTemplates.DataBind();
    }

  }
}