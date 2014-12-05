<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditOrder.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Orders.EditOrder"
  MasterPageFile="../Shared/UmbracoTabView.Master" ValidateRequest="false" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<%@ Register Assembly="TeaCommerce.Umbraco.Application" Namespace="TeaCommerce.Umbraco.Application.Views.Shared.Partials"
  TagPrefix="tc" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlOrder" runat="server">
    <asp:Literal ID="LitXSLTContent" runat="server" />
    <umbUIControls:Pane ID="PnOrder" runat="server" Visible="false" />
  </asp:Panel>
  <umbUIControls:Pane ID="PnOrderStatus" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlOrderStatus" runat="server">
      <tc:OrderStatusSelector ID="OrderStatusSelectorControl" runat="server" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
  <umbUIControls:Pane ID="PnEmail" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlSendEmail" runat="server">
      <asp:DropDownList ID="DrpEmailTemplates" runat="server" DataTextField="Name" DataValueField="Id"
        AppendDataBoundItems="true" />
      <asp:Button ID="BtnSendEmail" runat="server" OnClick="BtnSendEmail_Click" Style="margin-left: 10px;" />
      <asp:Label ID="LblEmailSent" runat="server" Visible="false" CssClass="emailSent" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
  <umbUIControls:Pane ID="PnTransaction" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlTransactionPaymentName" runat="server">
      <asp:Label ID="LblTransactionPaymentName" runat="server" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlCartName" runat="server">
      <asp:Label ID="LblCartName" runat="server" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlTransactionStatus" runat="server">
      <asp:Label ID="LblTransactionStatus" runat="server" />
      <asp:Label ID="LblInconsistentPayment" runat="server" style="color: #982927" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlTransactionAmount" runat="server">
      <asp:Label ID="LblTransactionAmount" runat="server" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlTransactionId" runat="server">
      <asp:Label ID="LblTransactionId" runat="server" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlError" runat="server" Visible="false">
      <asp:Label ID="LblError" runat="server" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel runat="server" Text=" ">
      <asp:Button ID="BtnFinalize" runat="server" OnClick="BtnFinalize_Click" Visible="false"
        CssClass="btnFinalize" />
      <asp:Button ID="BtnCancel" runat="server" OnClick="BtnCancel_Click" Visible="false"
        CssClass="btnCancel" />
      <asp:Button ID="BtnCapture" runat="server" OnClick="BtnCapture_Click" Visible="false"
        CssClass="btnCapture" />
      <asp:Button ID="BtnApprove" runat="server" OnClick="BtnApprove_Click" Visible="false"
        CssClass="btnApprove" />
      <asp:Button ID="BtnRefund" runat="server" OnClick="BtnRefund_Click" Visible="false"
        CssClass="btnRefund" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
</asp:Content>
<asp:Content ContentPlaceHolderID="CphHead" runat="server">
  <style type="text/css" media="print">
    .dontPrint { display: none; }
    .onlyPrint { display: block !important; }
    
    #body_TabViewControl { height: auto !important; width: auto !important; }
    .header, .menubar, .footer { display: none; }
    .tabpagecontainer { border-left: none; border-right: none; }
    .tabpagecontainer .tabpagescrollinglayer { height: auto !important; width: auto !important; }
    .tabpagecontainer .tabpagescrollinglayer .tabpageContent { padding: 0; }
    .tabpagecontainer .tabpagescrollinglayer .propertypane { background: none; }
  </style>
  <script type="text/javascript">
    saveSubscribers = [];

    var onSaving = function (method) {
      saveSubscribers.push(method);
    };

    function save() {
      for (var i = 0; i < saveSubscribers.length; i++) {
        saveSubscribers[i]();
      }
    }

    jQuery(function () {
      jQuery(".btnSave").click(save);
      jQuery(".btnDelete").click(function () {
        if (!confirm('<%= TeaCommerce.Umbraco.Application.Resources.CommonTerms.ConfirmDeleteOrder %>')) {
          return false;
        }
      });
      jQuery(".btnCancel").click(function () {
        if (!confirm('<%= TeaCommerce.Umbraco.Application.Resources.PaymentProviderTerms.ConfirmPaymentCancel %>')) {
          return false;
        }
      });
      jQuery(".btnCapture").click(function () {
        if (!confirm('<%= TeaCommerce.Umbraco.Application.Resources.PaymentProviderTerms.ConfirmPaymentCapture %>')) {
          return false;
        }
      });
      jQuery(".btnRefund").click(function () {
        if (!confirm('<%= TeaCommerce.Umbraco.Application.Resources.PaymentProviderTerms.ConfirmPaymentRefund %>')) {
          return false;
        }
      });

      var test = jQuery(".emailSent");
      if (test[0]) {
        window.setTimeout(function () {
          test.animate({ opacity: 0 }, 500);
        }, 2000);
      }

    });

  </script>
</asp:Content>
