﻿<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditCountry.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Countries.EditCountry"
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
    <umbUIControls:PropertyPanel ID="PPnlStandardCurrency" runat="server">
      <asp:DropDownList ID="DrpCurrencies" runat="server" DataTextField="Name" DataValueField="Id"
        CssClass="guiInputText guiInputStandardSize" />
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
  <script>
    var queryString = window.location.getParams();
    var storeId = queryString["storeId"];
    var id = queryString["id"];
    var path = [
      "-1",
      "Store_" + storeId + "_" + storeId,
      "Settings_" + storeId,
      "SettingsInternationalization_" + storeId,
      "SettingsCountries_" + storeId,
      "SettingsCountry_" + storeId + "_" + id
    ];
    UmbClientMgr.mainTree().setActiveTreeType("tea-commerce-store-tree");
    UmbClientMgr.mainTree().syncTree(path.join(","), true);
  </script>
</asp:Content>
