<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditOrderStatus.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.OrderStatuses.EditOrderStatus"
  MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <umbUIControls:Pane ID="PnCommon" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlName" runat="server">
      <asp:TextBox ID="TxtName" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlDictionaryItemName" runat="server">
      <asp:TextBox ID="TxtDictionaryItemName" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlRecalculateFinalizedOrder" runat="server">
      <asp:CheckBox ID="ChkRecalculateFinalizedOrder" runat="server" />
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
      "SettingsOrderStatuses_" + storeId,
      "SettingsOrderStatus_" + storeId + "_" + id
    ];
    UmbClientMgr.mainTree().setActiveTreeType("tea-commerce-store-tree");
    UmbClientMgr.mainTree().syncTree(path.join(","), true);
  </script>
</asp:Content>
