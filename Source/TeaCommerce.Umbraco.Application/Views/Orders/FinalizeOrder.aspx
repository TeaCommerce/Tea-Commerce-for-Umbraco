<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalizeOrder.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.Orders.FinalizeOrder" MasterPageFile="~/Umbraco/masterpages/umbracoDialog.Master" %>

<asp:Content ContentPlaceHolderID="body" runat="server">
  <div style="margin-top: 10px;">
    <asp:Literal ID="LitTransactionId" runat="server" />:<br />
    <asp:TextBox ID="TxtTransactionId" runat="server" CssClass="bigInput" Width="350px" />
    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="TxtTransactionId" />
    <div style="margin-top: 10px;">
      <asp:Literal ID="LitAmount" runat="server" />:
      <br />
      <asp:TextBox ID="TxtAmount" runat="server" CssClass="bigInput" Width="350px" />
      <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="TxtAmount" />
    </div>
    <br />
    <!-- added to support missing postback on enter in IE -->
    <asp:TextBox runat="server" Style="visibility: hidden; display: none;" />
  </div>
  <div style="padding-top: 25px;">
    <asp:Button ID="BtnFinalize" runat="server" OnClick="BtnFinalize_Click" Style="width: 90px" />
    &nbsp; <em>
      <asp:Literal ID="LitOr" runat="server" /></em> &nbsp; <a href="#" style="color: blue"
        onclick="UmbClientMgr.closeModalWindow()">
        <asp:Literal ID="LitCancel" runat="server" /></a>
  </div>
</asp:Content>