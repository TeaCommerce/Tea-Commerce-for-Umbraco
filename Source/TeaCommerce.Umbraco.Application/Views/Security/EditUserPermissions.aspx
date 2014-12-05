<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditUserPermissions.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.Security.EditUserPermissions" MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlCommon" runat="server">
    <umbUIControls:Pane runat="server">
      <umbUIControls:PropertyPanel ID="PPnlAccessSecurity" runat="server">
        <asp:Image ID="ImgAccessSecurity" runat="server" />
        <asp:CheckBox ID="ChkAccessSecurity" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlAccessLicenses" runat="server">
        <asp:Image ID="ImgAccessLicenses" runat="server" />
        <asp:CheckBox ID="ChkAccessLicenses" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlCreateAndDeleteStore" runat="server">
        <asp:Image ID="ImgCreateAndDeleteStore" runat="server" />
        <asp:CheckBox ID="ChkCreateAndDeleteStore" runat="server" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
    <umbUIControls:Pane ID="PnStoreSpecificPermissions" runat="server">
      <umbUIControls:PropertyPanel ID="PPnlStoreSpecificPermissions" runat="server">
        <asp:ListView ID="LvStores" runat="server" OnItemDataBound="LvStores_ItemDataBound">
          <LayoutTemplate>
            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
          </LayoutTemplate>
          <ItemTemplate>
            <asp:HiddenField ID="HdfId" runat="server" Value='<%# Eval("Id") %>' />
            <div style="padding-bottom: 5px;">
              <asp:Image ID="ImgAccessStore" runat="server" />
              <asp:CheckBox ID="ChkAccessStore" runat="server" />
            </div>
            <asp:Panel ID="PnlMarketing" runat="server" style="padding: 0 0 5px 20px;">
              <asp:Image ID="ImgMarketing" runat="server" />
              <asp:CheckBox ID="ChkMarketing" runat="server" />
            </asp:Panel>
            <asp:Panel ID="PnlAccessSettings" runat="server" style="padding: 0 0 5px 20px;">
              <asp:Image ID="ImgAccessSettings" runat="server" />
              <asp:CheckBox ID="ChkAccessSettings" runat="server" />
            </asp:Panel>
          </ItemTemplate>
        </asp:ListView>
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
  </asp:Panel>
</asp:Content>
