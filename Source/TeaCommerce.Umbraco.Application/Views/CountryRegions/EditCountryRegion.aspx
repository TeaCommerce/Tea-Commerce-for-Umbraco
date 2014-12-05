<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditCountryRegion.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.CountryRegions.EditCountryRegion"
  MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphBody" runat="server">
  <umbUIControls:Pane ID="PnCommon" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlName" runat="server">
      <asp:TextBox ID="TxtName" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlRegionCode" runat="server">
      <asp:TextBox ID="TxtRegionCode" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlStandardShippingMethod" runat="server">
      <asp:DropDownList ID="DrpShippingMethods" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlStandardPaymentMethod" runat="server">
      <asp:DropDownList ID="DrpPaymentMethods" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true" />
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
</asp:Content>
