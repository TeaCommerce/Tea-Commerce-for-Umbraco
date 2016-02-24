<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAll.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.CreateAll.CreateAll" MasterPageFile="~/Umbraco/masterpages/umbracoDialog.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  <div id="CreateControls" runat="server">
    <div style="margin-top: 20px;">
      <asp:Literal ID="LitCreateAllWarning" runat="server" />
      <div style="margin-top: 10px;">
        <asp:Literal ID="LitDefaultCurrency" runat="server" />:<br />
        <asp:DropDownList ID="DrpCurrencies" runat="server" DataTextField="Name" DataValueField="Id" CssClass="bigInput" Width="350px" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="DrpCurrencies" />
      </div>
    </div>
    <div style="padding-top: 25px;">
      <asp:Button ID="BtnCreate" runat="server" OnClick="BtnCreate_Click" Style="width: 90px" />
      <em>
        <asp:Literal ID="LitOr" runat="server" />
      </em>&nbsp;
    <a href="#" style="color: blue" onclick="UmbClientMgr.closeModalWindow()">
      <asp:Literal ID="LitCancel" runat="server" />
    </a>
    </div>
  </div>
  <div ID="CreateAllCompleted" Visible="False" runat="server">
    <div style="margin-top: 20px;">
      <asp:Literal ID="LitCreateAllComplete" runat="server" />
    </div>
  </div>
</asp:Content>
