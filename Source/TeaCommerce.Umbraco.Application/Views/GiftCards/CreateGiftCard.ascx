<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateGiftCard.ascx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.GiftCards.CreateGiftCard" %>
<div style="margin-top: 20px;">
  <asp:Literal ID="LitAmount" runat="server" />:<br />
  <asp:TextBox ID="TxtAmount" runat="server" CssClass="bigInput" Width="350px" />    
  <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="TxtAmount" />
  <div style="margin-top: 10px;">
    <asp:Literal ID="LitCurrency" runat="server" />:
    <br />
    <asp:DropDownList ID="DrpCurrencies" runat="server" DataTextField="Name" DataValueField="Id"
      CssClass="bigInput" Width="350px" />
    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="DrpCurrencies" />
  </div>
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
