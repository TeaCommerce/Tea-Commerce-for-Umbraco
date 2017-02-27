<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="GiftCardOverview.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.GiftCards.GiftCardOverview"
  MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Import Namespace="TeaCommerce.Umbraco.Application" %>
<%@ Import Namespace="TeaCommerce.Umbraco.Application.Resources" %>
<%@ Import Namespace="TeaCommerce.Umbraco.Application.Utils" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols.DatePicker" TagPrefix="umbUIControlsDatePicker" %>
<%@ Register TagPrefix="tc" Namespace="TeaCommerce.Umbraco.Application.UI" Assembly="TeaCommerce.Umbraco.Application" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlGiftCard" runat="server">
    <asp:Panel ID="PnlLicenseCheck" runat="server" CssClass="licenseCheck">
      <asp:Literal ID="LitTrialMode" runat="server"></asp:Literal>
      <asp:HyperLink ID="HypTrialMode" runat="server" NavigateUrl="http://www.teacommerce.net" Target="_blank"></asp:HyperLink>
    </asp:Panel>
    <umbUIControls:Pane ID="PnSearchCriteria" runat="server">
        <umbUIControls:PropertyPanel ID="PPnlCode" runat="server">
        <asp:TextBox ID="TxtCode" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlOrderNumer" runat="server">
        <asp:TextBox ID="TxtOrderNumber" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlCurrency" runat="server">
        <asp:DropDownList ID="DrpCurrencies" runat="server" DataTextField="Name" DataValueField="Id"
          CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true">
          <asp:ListItem Text="----" Value="" />
        </asp:DropDownList>
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlStartDate" runat="server">
        <umbUIControlsDatePicker:DateTimePicker ID="DPStart" runat="server" ShowTime="False" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlEndDate" runat="server">
        <umbUIControlsDatePicker:DateTimePicker ID="DPEnd" runat="server" ShowTime="False" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel runat="server" Text=" ">
        <asp:Button ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" />
        <asp:Button ID="BtnReset" runat="server" OnClick="BtnReset_Click" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
    <umbUIControls:Pane ID="PnSearchResult" runat="server">
      <tc:DataboundListView ID="LvGiftCards" runat="server">
        <LayoutTemplate>
          <table width="100%" rules="rows">
            <thead>
              <th style="text-align: left;"><%# CommonTerms.Code %></th>              
              <th style="text-align: left;"><%# CommonTerms.OriginalAmount %></th>
              <th style="text-align: left;"><%# CommonTerms.UsedAmount %></th>
              <th style="text-align: left;"><%# CommonTerms.RemainingAmount %></th>
              <th style="text-align: left;"><%# CommonTerms.Currency %></th>
              <th style="text-align: left;"><%# CommonTerms.DaysRemaining %></th>
            </thead>
            <tbody>
              <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
            </tbody>
          </table>
        </LayoutTemplate>
        <ItemTemplate>
          <tr>
            <td><asp:HyperLink runat="server" Text='<%# Eval( "Code" ) %>' NavigateUrl='<%# WebUtils.GetPageUrl( TeaCommerce.Umbraco.Application.Constants.Pages.EditGiftCard, false ) + "?storeId=" + StoreId + "&id=" + Eval("Id") %>' /></td>
            <td><%# ((decimal)Eval( "OriginalAmount" )).ToString("0.####") %></td>
            <td><%# ((decimal)Eval( "UsedAmount" )).ToString("0.####") %></td>
            <td><%# ((decimal)Eval( "RemainingAmount" )).ToString("0.####") %></td>
            <td><%# Eval( "Currency" ) %></td>
            <td><%# Eval( "DaysRemaining" ) %></td>
          </tr>
        </ItemTemplate>
      </tc:DataboundListView>      
    </umbUIControls:Pane>
  </asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="CphHead" runat="server">
  <style type="text/css">
    .licenseCheck { background: #FFF6BF; margin: 10px 0; padding: 20px; color: #5E532C; text-align: center; }
    .umbDateTimePicker button { background-image: url(<%= TeaCommerce.Umbraco.Application.Utils.WebUtils.GetWebResourceUrl(TeaCommerce.Umbraco.Application.Constants.EditorIcons.Calendar) %>) !important;}
  </style>
</asp:Content>