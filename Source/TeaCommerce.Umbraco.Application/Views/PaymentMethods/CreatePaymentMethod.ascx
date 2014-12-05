<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreatePaymentMethod.ascx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.PaymentMethods.CreatePaymentMethod" %>
<div style="margin-top: 20px;">
  <asp:Literal ID="LitName" runat="server" />:<br />
  <asp:TextBox ID="TxtName" runat="server" CssClass="bigInput" Width="350px" />
  <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="TxtName" />
  <!-- added to support missing postback on enter in IE -->
  <asp:TextBox runat="server" Style="visibility: hidden; display: none;" />
</div>
<div style="padding-top: 25px;">
  <asp:Button ID="BtnCreate" runat="server" OnClick="BtnCreate_Click" Style="width: 90px" />
  &nbsp; <em>
    <asp:Literal ID="LitOr" runat="server" /></em> &nbsp; <a href="#" style="color: blue"
      onclick="UmbClientMgr.closeModalWindow()">
      <asp:Literal ID="LitCancel" runat="server" /></a>
</div>
