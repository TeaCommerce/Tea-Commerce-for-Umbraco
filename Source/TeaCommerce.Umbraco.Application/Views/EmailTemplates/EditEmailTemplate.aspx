<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditEmailTemplate.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.EmailTemplates.EditEmailTemplate"
  MasterPageFile="../Shared/UmbracoTabView.Master" ValidateRequest="false" %>

<%@ Register Assembly="TeaCommerce.Umbraco.Application" Namespace="TeaCommerce.Umbraco.Application.Views.Shared.Partials"
  TagPrefix="tc" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel runat="server" ID="PnCommon">
    <umbUIControls:Pane ID="PnEmailTemplate" runat="server">
      <umbUIControls:PropertyPanel ID="PPnlName" runat="server">
        <asp:TextBox ID="TxtName" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlAlias" runat="server">
        <asp:TextBox ID="TxtAlias" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlShouldClientReceiveEmail" runat="server">
        <asp:CheckBox ID="ChkShouldClientReceiveEmail" runat="server" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
    <umbUIControls:Pane ID="PnDefaultSettings" runat="server">
      <umbUIControls:PropertyPanel ID="PPnlSubject" runat="server">
        <asp:TextBox ID="TxtSubject" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlSenderName" runat="server">
        <asp:TextBox ID="TxtSenderName" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlSenderAddress" runat="server">
        <asp:TextBox ID="TxtSenderAddress" runat="server" CssClass="guiInputText guiInputStandardSize" />
        <asp:CustomValidator ID="CusValSenderAddress" runat="server" ControlToValidate="TxtSenderAddress"
          OnServerValidate="Email_ServerValidate" ErrorMessage="*" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlToAddresses" runat="server">
        <asp:TextBox ID="TxtToAddresses" runat="server" CssClass="guiInputText guiInputStandardSize" />
        <asp:CustomValidator ID="CusValToAddresses" runat="server" ControlToValidate="TxtToAddresses"
          OnServerValidate="Email_ServerValidate" ErrorMessage="*" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlCCAddresses" runat="server">
        <asp:TextBox ID="TxtCCAddresses" runat="server" CssClass="guiInputText guiInputStandardSize" />
        <asp:CustomValidator ID="CusValCCAddresses" runat="server" ControlToValidate="TxtCCAddresses"
          OnServerValidate="Email_ServerValidate" ErrorMessage="*" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlBCCAddresses" runat="server">
        <asp:TextBox ID="TxtBCCAddresses" runat="server" CssClass="guiInputText guiInputStandardSize" />
        <asp:CustomValidator ID="CusValBCCAddresses" runat="server" ControlToValidate="TxtBCCAddresses"
          OnServerValidate="Email_ServerValidate" ErrorMessage="*" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlTemplate" runat="server">
        <tc:TemplateFileSelector ID="TemplateFileSelectorControl" runat="server" InsertEmptyItem="true" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
  </asp:Panel>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CphHead" runat="server">
  <style type="text/css">
    .notice { margin-top: 7px; padding: 5px; }
  </style>
</asp:Content>
