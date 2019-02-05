<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditUserPermissions.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.Security.EditUserPermissions" MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<asp:Content ContentPlaceHolderID="CphHead" runat="server">
<style>
  .icon { font-size: 20px; margin-right: 2px; vertical-align: middle; }
  input[type=checkbox] { vertical-align: middle; margin-top: 0; margin-right: 5px; }
  input[type=checkbox] + label { vertical-align: middle; margin-bottom: 0; padding: 0; }
</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlCommon" runat="server">
    <umbUIControls:Pane runat="server">
      <umbUIControls:PropertyPanel ID="PPnlAccessSecurity" runat="server">
        <i class="icon icon-lock"></i>
        <asp:CheckBox ID="ChkAccessSecurity" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlAccessLicenses" runat="server">
          <i class="icon icon-certificate"></i>
          <asp:CheckBox ID="ChkAccessLicenses" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlCreateAndDeleteStore" runat="server">
          <i class="icon icon-store"></i>
          <asp:CheckBox ID="ChkCreateAndDeleteStore" runat="server" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
    <umbUIControls:Pane ID="PnStoreSpecificPermissions" runat="server">
      <umbUIControls:PropertyPanel ID="PPnlStoreSpecificPermissions" runat="server">
        <asp:ListView ID="LvStores" runat="server" OnItemDataBound="LvStores_ItemDataBound">
          <LayoutTemplate>
            <div style="margin-top: -20px;">
              <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
            </div>
          </LayoutTemplate>
          <ItemTemplate>
            <asp:HiddenField ID="HdfId" runat="server" Value='<%# Eval("Id") %>' />
            <div style="padding-bottom: 5px; margin-top: 20px;">
              <i class="icon icon-store"></i>
              <asp:CheckBox ID="ChkAccessStore" runat="server" />
            </div>
            <asp:Panel ID="PnlMarketing" runat="server" style="padding: 0 0 5px 20px;">
                <i class="icon icon-target"></i>
                <asp:CheckBox ID="ChkMarketing" runat="server" />
            </asp:Panel>
            <asp:Panel ID="PnlAccessSettings" runat="server" style="padding: 0 0 5px 20px;">
                <i class="icon icon-settings"></i>
                <asp:CheckBox ID="ChkAccessSettings" runat="server" />
            </asp:Panel>
          </ItemTemplate>
        </asp:ListView>
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
  </asp:Panel>
  <script>
    var queryString = window.location.getParams();
    UmbClientMgr.mainTree().setActiveTreeType("tea-commerce-security-tree");
    UmbClientMgr.mainTree().syncTree("-1,User_" + queryString["id"]);
  </script>
</asp:Content>
