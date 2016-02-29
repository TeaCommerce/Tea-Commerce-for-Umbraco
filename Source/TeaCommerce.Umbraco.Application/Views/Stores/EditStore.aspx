<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditStore.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Stores.EditStore"
  MasterPageFile="../Shared/UmbracoTabView.Master" ValidateRequest="false" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<%@ Register Assembly="TeaCommerce.Umbraco.Application" Namespace="TeaCommerce.Umbraco.Application.Views.Shared.Partials"
  TagPrefix="tc" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <umbUIControls:Pane ID="PnCommon" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlName" runat="server">
      <asp:TextBox ID="TxtName" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlDefaultCountry" runat="server">
      <asp:DropDownList ID="DrpCountries" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlDefaultVatGroup" runat="server">
      <asp:DropDownList ID="DrpVatGroups" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlDefaultOrderStatus" runat="server">
      <asp:DropDownList ID="DrpOrderStatuses" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlConfirmationEmail" runat="server">
      <asp:DropDownList ID="DrpConfirmationEmail" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true">
        <asp:ListItem Text="----" Value="" />
      </asp:DropDownList>
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlPaymentInconsistencyEmail" runat="server">
      <asp:DropDownList ID="DrpPaymentInconsistencyEmail" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true">
        <asp:ListItem Text="----" Value="" />
      </asp:DropDownList>
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlPricesIsSpecifiedWithVat" runat="server">
      <asp:CheckBox ID="ChkPricesIsSpecifiedWithVat" runat="server" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlChkPersistOrderId" runat="server">
      <asp:CheckBox ID="ChkPersistOrderId" runat="server" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlChkOrderPersistanceTimeout" runat="server">
      <asp:TextBox ID="TxtOrderPersistanceTimeout" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
  <umbUIControls:Pane ID="PnOrder" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlCartNumberPrefix" runat="server">
      <asp:TextBox ID="TxtCartNumberPrefix" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlOrderNumberPrefix" runat="server">
      <asp:TextBox ID="TxtOrderNumberPrefix" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
  <umbUIControls:Pane ID="PnProduct" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlProductPropertyAliases" runat="server">
      <asp:TextBox ID="TxtProductPropertyAliases" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlProductUniquenessPropertyAliases" runat="server">
      <asp:TextBox ID="TxtProductUniquenessPropertyAliases" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlProductVariantPropertyAlias" runat="server">
      <asp:TextBox ID="TxtProductVariantPropertyAlias" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlStockSharingStore" runat="server">
      <asp:DropDownList ID="DrpStockSharingStore" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true">
        <asp:ListItem Text="----" Value="" />
      </asp:DropDownList>
      <asp:Label ID="LblStockSharingStoreAssigned" runat="server" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
  <umbUIControls:Pane ID="PnGiftCard" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlGiftCardLength" runat="server">
      <asp:TextBox ID="TxtGiftCardLength" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlGiftCardDaysValid" runat="server">
      <asp:TextBox ID="TxtGiftCardDaysValid" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlGiftCardPrefix" runat="server">
      <asp:TextBox ID="TxtGiftCardPrefix" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlGiftCardSuffix" runat="server">
      <asp:TextBox ID="TxtGiftCardSuffix" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
  <umbUIControls:Pane ID="PnTemplateRendering" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlEditOrderUIFile" runat="server">
      <tc:TemplateFileSelector ID="TemplateFileSelectorControl" runat="server" InsertEmptyItem="true" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlTemplateRendering" runat="server">
      <tc:TemplateFileSelectionList ID="TemplateFileSelectionListControl" runat="server" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CphHead" runat="server">
  <script type="text/javascript">
    jQuery(function () {
      setCookieTimeoutField(jQuery('#body_ChkPersistOrderId'));
    });

    jQuery('#body_ChkPersistOrderId').live('click', function () {
      setCookieTimeoutField(jQuery(this));
    });

    function setCookieTimeoutField(checkbox) {
      var propertyItem = checkbox.closest('.umb-el-wrap').next('.umb-el-wrap');
      propertyItem.css({ display: checkbox.is(':checked') ? 'block' : 'none' });
    }

  </script>
</asp:Content>
