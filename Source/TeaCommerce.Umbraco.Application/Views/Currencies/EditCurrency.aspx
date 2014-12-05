<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditCurrency.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Currencies.EditCurrency"
  MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="TeaCommerce.Umbraco.Application" Namespace="TeaCommerce.Umbraco.Application.Views.Shared.Partials"
  TagPrefix="tc" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <umbUIControls:Pane ID="PnCommon" runat="server">
    <umbUIControls:PropertyPanel ID="PPnlName" runat="server">
      <asp:TextBox ID="TxtName" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlIsoCode" runat="server">
      <asp:TextBox ID="TxtIsoCode" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlPriceField" runat="server">
      <asp:TextBox ID="TxtPriceField" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlCulture" runat="server">
      <tc:CultureCodeSelector ID="CultureCodeSelectorControl" runat="server" CssClass="guiInputText guiInputStandardSize" />
    </umbUIControls:PropertyPanel>
    <umbUIControls:PropertyPanel ID="PPnlSpecialSymbol" runat="server">
      <asp:CheckBox ID="ChkSpecialSymbol" runat="server" CssClass="specialSymbol" />
    </umbUIControls:PropertyPanel>
    <div id="specialSymbolContainer">
      <umbUIControls:PropertyPanel ID="PPnlSymbol" runat="server">
        <asp:TextBox ID="TxtSymbol" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlSymbolPlacement" runat="server">
        <asp:RadioButtonList ID="RdbListSymbolPlacement" runat="server" />
      </umbUIControls:PropertyPanel>
    </div>
  </umbUIControls:Pane>
  <umbUIControls:Pane ID="PnCountries" runat="server">
    <umbUIControls:PropertyPanel runat="server">
      <asp:CheckBox runat="server" ID="ChkSelectAll" CssClass="selectAll" />
      <div class="countries checkboxesForSelectAll">
        <asp:CheckBoxList ID="ChkLstCountries" runat="server" DataValueField="Id" DataTextField="Name" />
      </div>
    </umbUIControls:PropertyPanel>
  </umbUIControls:Pane>
</asp:Content>
<asp:Content ContentPlaceHolderID="CphHead" runat="server">
  <style type="text/css">
    .selectAll { font-size: 11px; display: inline-block; padding-left: 3px; margin-bottom: 5px; }
  </style>
  <script type="text/javascript" src='<%= TeaCommerce.Umbraco.Application.Utils.WebUtils.GetWebResourceUrl(TeaCommerce.Umbraco.Application.Constants.Scripts.Default) %>'></script>
</asp:Content>
