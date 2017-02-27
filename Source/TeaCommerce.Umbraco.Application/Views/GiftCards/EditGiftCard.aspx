<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditGiftCard.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.GiftCards.EditGiftCard"
  MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols.DatePicker" TagPrefix="umbUIControlsDatePicker" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlGiftCard" runat="server">
    <umbUIControls:Pane ID="PnGiftCardOverview" runat="server">
      <umbUIControls:PropertyPanel ID="PPnlCode" runat="server" Text="GiftCardCode">
        <asp:Literal ID="LitCode" runat="server"></asp:Literal>
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlOriginalAmount" runat="server" Text="GiftCardOriginalAmount">
        <asp:TextBox ID="TxtOriginalAmount" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlRemainingAmount" runat="server" Text="GiftCardRemainingAmount">
        <asp:TextBox ID="TxtRemainingAmount" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlCurrency" runat="server" Text="GiftCardCurrency">
        <asp:DropDownList ID="DrpCurrencies" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlValidTo" runat="server">
        <umbUIControlsDatePicker:DateTimePicker ID="DPValidTo" runat="server" ShowTime="False" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
  </asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="CphHead" runat="server">
  <style type="text/css">
    .umbDateTimePicker button { background-image: url(<%= TeaCommerce.Umbraco.Application.Utils.WebUtils.GetWebResourceUrl(TeaCommerce.Umbraco.Application.Constants.EditorIcons.Calendar) %>) !important;}
  </style>
</asp:Content>