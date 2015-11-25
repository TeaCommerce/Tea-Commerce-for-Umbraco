<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="NeedHelp.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Developer.NeedHelp"
  MasterPageFile="../Shared/UmbracoTabView.Master" ValidateRequest="false" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <style type="text/css">
    .teaCommerce { padding: 10px 10px 0; }
    .teaCommerce div.codepress { border: 1px solid #ccc !important; height: 72px; }
    .teaCommerce div.codepress > div { height: 100%; }
    .teaCommerce .teaLogo { display: block; margin: 0 0 25px 15px; }
    .teaCommerce h2.propertypaneTitel { margin-bottom: 10px; color: #000; }
    .teaCommerce ul.links li { padding-bottom: 4px; }
  </style>
  <asp:Panel ID="PnlGetStarted" runat="server">
    <umbUIControls:Pane runat="server" ID="sortPane">
      <div class="teaCommerce">
        <h2 class="propertypaneTitel">
          <asp:Literal ID="LitUsefulLinks" runat="server" /></h2>
        <p>
          <ul class="links">
            <li><a href="http://www.teacommerce.net/en/support/how-to-buy-a-license.aspx" target="_blank">
              <asp:Literal ID="LitBuyALicense" runat="server" /></a></li>
            <li><a href="https://documentation.teacommerce.net/" target="_blank">
              <asp:Literal ID="LitDocumentation" runat="server" /></a></li>
            <li><a href="http://our.umbraco.org/projects/website-utilities/tea-commerce/tea-commerce-support"
              target="_blank">
              <asp:Literal ID="LitSupportForum" runat="server" /></a></li>
            <li><a href="http://www.teacommerce.net/en/products/tea-commerce-starter-kit.aspx"
              target="_blank">
              <asp:Literal ID="LitStarterKit" runat="server" /></a></li>
            <li><a href="http://www.teacommerce.net/en/documentation/revision-history.aspx" target="_blank">
              <asp:Literal ID="LitRevisionHistory" runat="server" /></a></li>
          </ul>
        </p>
      </div>
    </umbUIControls:Pane>
  </asp:Panel>
</asp:Content>
