<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateCountry.ascx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Countries.CreateCountry" %>
<div style="margin-top: 20px;">
  <asp:Literal ID="LitSelectCountry" runat="server" />:<br />
  <asp:DropDownList ID="DrpCountries" runat="server" DataTextField="Name" DataValueField="Code" CssClass="bigInput" Width="350px" /><br />
  <asp:Literal ID="LitOrTypeName" runat="server" />:<br />
  <asp:TextBox ID="TxtName" runat="server" CssClass="bigInput" Width="350px" />
  <div style="margin-top: 10px;">
    <asp:Literal ID="LitDefaultCurrency" runat="server" />:
    <br />
    <asp:DropDownList ID="DrpCurrencies" runat="server" DataTextField="Name" DataValueField="Id"
      CssClass="bigInput" Width="350px" />
    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="DrpCurrencies" />
  </div>
  <!-- added to support missing postback on enter in IE -->
  <asp:TextBox runat="server" Style="visibility: hidden; display: none;" />
</div>
<div style="padding-top: 25px;">
  <asp:Button ID="BtnCreate" runat="server" OnClick="BtnCreate_Click" Style="width: 90px" />&nbsp;
  <em><asp:Literal ID="LitOr" runat="server" /></em>&nbsp;
  <a href="#" style="color: blue" onclick="UmbClientMgr.closeModalWindow()">
  <asp:Literal ID="LitCancel" runat="server" /></a>
</div>
