<%@ Page Title="" Language="C#" MasterPageFile="../Shared/UmbracoTabView.Master"
  AutoEventWireup="True" CodeBehind="SearchOrders.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Orders.SearchOrders" %>

<%@ Import Namespace="TeaCommerce.Api.Models" %>
<%@ Import Namespace="TeaCommerce.Api.Services" %>
<%@ Import Namespace="TeaCommerce.Umbraco.Application" %>
<%@ Import Namespace="TeaCommerce.Umbraco.Application.Resources" %>
<%@ Import Namespace="TeaCommerce.Umbraco.Application.Utils" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols.DatePicker" TagPrefix="umbUIControlsDatePicker" %>
<%@ Register Assembly="TeaCommerce.Umbraco.Application" Namespace="TeaCommerce.Umbraco.Application.Views.Shared.Partials"
  TagPrefix="tc" %>
<%@ Register TagPrefix="tc" Namespace="TeaCommerce.Umbraco.Application.UI" Assembly="TeaCommerce.Umbraco.Application" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlSearch" runat="server">
    <asp:Panel ID="PnlLicenseCheck" runat="server" CssClass="licenseCheck">
      <asp:Literal ID="LitTrialMode" runat="server"></asp:Literal>
      <asp:HyperLink ID="HypTrialMode" runat="server" NavigateUrl="http://www.teacommerce.net" Target="_blank"></asp:HyperLink>
    </asp:Panel>
    <umbUIControls:Pane ID="PnSearchCriteria" runat="server">
      <umbUIControls:PropertyPanel ID="PPnlOrderNumber" runat="server">
        <asp:TextBox ID="TxtOrderNumber" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlFirstName" runat="server">
        <asp:TextBox ID="TxtFirstName" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlLastName" runat="server">
        <asp:TextBox ID="TxtLastName" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlPaymentStatus" runat="server">
        <tc:PaymentStatusSelector ID="PaymentStatusSelectorControl" runat="server" InsertEmptyItem="true" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlOrderStage" runat="server">
        <asp:DropDownList ID="DrpOrderStages" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlPageSize" runat="server">
        <asp:DropDownList ID="DrpPageSize" runat="server" CssClass="guiInputText guiInputStandardSize">
          <asp:ListItem Text="10" Value="10" />
          <asp:ListItem Text="25" Value="25" Selected="True" />
          <asp:ListItem Text="50" Value="50" />
          <asp:ListItem Text="75" Value="75" />
          <asp:ListItem Text="100" Value="100" />
          <asp:ListItem Text="200" Value="200" />
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
    <div id="teaTools">
      <umbUIControls:Pane ID="PnTools" runat="server" CssClass="">
        <umbUIControls:PropertyPanel ID="PPnlDeleteOrders" runat="server">
          <asp:Button ID="BtnDeleteOrders" runat="server" OnClick="BtnDeleteOrders_Click" CssClass="btnDeleteOrders" />
          <asp:CheckBox ID="ChkRevertFinalize" Checked="True" runat="server" />
        </umbUIControls:PropertyPanel>
        <umbUIControls:PropertyPanel ID="PPnlCapturePayments" runat="server">
          <asp:Button ID="BtnCapturePayments" runat="server" OnClick="BtnCapturePayments_Click" CssClass="btnCapturePayments" />
        </umbUIControls:PropertyPanel>
        <umbUIControls:PropertyPanel ID="PPnlChangeOrderStatus" runat="server">
          <tc:OrderStatusSelector ID="OrderStatusSelectorControl" runat="server" />
          <asp:Button ID="BtnChangeOrderStatus" runat="server" OnClick="BtnChangeOrderStatus_Click" />
        </umbUIControls:PropertyPanel>
      </umbUIControls:Pane>
    </div>
    <umbUIControls:Pane ID="PnSearchResult" runat="server" Visible="false">
      <tc:DataboundListView ID="LvOrders" runat="server">
        <Layouttemplate>
          <table width="100%" rules="rows">
            <thead>
              <tr>
                <th style="width: 30px; text-align: left;">
                  <input type="checkbox" id="selectAll" />
                </th>
                <th style="width: 150px; text-align: left;">
                  <%# CommonTerms.OrderNumer %>
                </th>
                <th style="width: 335px; text-align: left;">
                  <%# CommonTerms.Customer %>
                </th>
                <th style="width: 200px; text-align: left;">
                  <%# CommonTerms.Created %>
                </th>
                <th style="width: 125px; text-align: left;">
                  <%# CommonTerms.PaymentMethod %>
                </th>
                <th style="width: 175px; text-align: left;">
                  <%# CommonTerms.PaymentState %>
                </th>
                <th style="width: 130px; text-align: left;">
                  <%# CommonTerms.Price %>
                </th>
              </tr>
            </thead>
            <tbody>
              <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
            </tbody>
          </table>
        </Layouttemplate>
        <Itemtemplate>
          <tr>
            <td>
              <asp:CheckBox ID="ChkSelect" runat="server" CssClass="orderSelect" />
              <asp:HiddenField ID="HdnId" runat="server" Value='<%# Eval("Id") %>' />
            </td>
            <td>
              <asp:HyperLink runat="server" Text='<%# (bool)Eval("IsFinalized" ) ? Eval("OrderNumber" ) : Eval("CartNumber" ) %>' NavigateUrl='<%# WebUtils.GetPageUrl( TeaCommerce.Umbraco.Application.Constants.Pages.EditOrder, false ) + "?storeId=" + StoreId + "&id=" + Eval("Id") %>' />
              <asp:Image ID="ImgExclamation" runat="server" ImageUrl='<%# WebUtils.GetWebResourceUrl( TeaCommerce.Umbraco.Application.Constants.MiscIcons.Exclamation ) %>' Visible='<%# Eval("TransactionInformation.InconsistentPayment") %>' style="position: relative; margin-left: 4px; top: 2px;"/>
            </td>
            <td>
              <%# Eval( "PaymentInformation.FirstName" ) + " " + Eval( "PaymentInformation.LastName" ) %>
            </td>
            <td>
              <%# ( (DateTime)( Eval( "DateFinalized" ) ?? Eval( "DateCreated" ) ) ).ToString( "ddd, dd MMM yyyy HH:mm" )%>
            </td>
            <td>
              <%# Eval( "PaymentInformation.PaymentMethodId" ) != null ? PaymentMethodService.Instance.Get(StoreId, (long)Eval( "PaymentInformation.PaymentMethodId" )).Name : "" %>
            </td>
            <td>
              <%# Eval( "TransactionInformation.PaymentState" ) != null ? CommonTerms.ResourceManager.GetString( ((PaymentState)Eval( "TransactionInformation.PaymentState" )).ToString()) : ""%>
              <asp:Label runat="server" Visible='<%# Eval("TransactionInformation.InconsistentPayment") %>' style="color: #982927;"> (<%# CommonTerms.InconsistentPayment %>)</asp:Label>
            </td>
            <td>
              <%# Eval("TotalPrice.Value.WithVatFormatted" ) %>
            </td>
          </tr>
        </Itemtemplate>
      </tc:DataboundListView>
      <asp:Panel ID="PnlPager" runat="server" CssClass="pager">
        <asp:LinkButton ID="LBtnFirstPage" runat="server" OnClick="LBtnFirstPage_Click" Text="|<" /><asp:LinkButton
          ID="LBtnPreviousPage" runat="server" OnClick="LBtnPreviousPage_Click" Text="<" />
        <asp:Literal ID="LitCurrentPage" runat="server" />&nbsp;/&nbsp;<asp:Literal ID="LitMaxPages"
          runat="server" /><asp:LinkButton ID="LBtnNextPage" runat="server" OnClick="LBtnNextPage_Click"
            Text=">" /><asp:LinkButton ID="LBtnLastPage" runat="server" OnClick="LBtnLastPage_Click"
              Text=">|" />
      </asp:Panel>
    </umbUIControls:Pane>
  </asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="CphHead" runat="server">
  <style type="text/css">
    .licenseCheck {
      background: #FFF6BF;
      margin: 10px 0;
      padding: 20px;
      color: #5E532C;
      text-align: center;
    }

    .pager {
      background: #E8E8E8;
      font-size: 11px;
      padding: 3px 15px 3px 0;
      text-align: right;
    }

      .pager a {
        display: inline-block;
        margin: 0px 3px;
        color: #000;
        text-decoration: none;
      }

        .pager a:hover {
          text-decoration: none;
        }

        .umbDateTimePicker button {
            background-image: url(<%= TeaCommerce.Umbraco.Application.Utils.WebUtils.GetWebResourceUrl(TeaCommerce.Umbraco.Application.Constants.EditorIcons.Calendar) %>) !important;
        }
  </style>
  <script type="text/javascript">
    jQuery(function () {
      jQuery('#teaTools input').attr('disabled', true);
    });

    jQuery('.orderSelect input').live('click', function () {
      var checkboxes = jQuery(this).closest('table').find('tr td input[type=checkbox]:checked');
      jQuery('#selectAll').attr('checked', false);
      if (checkboxes.length > 0) {
        jQuery('#teaTools input').attr('disabled', false);
      } else {
        jQuery('#teaTools input').attr('disabled', true);
      }
    });

    jQuery('#selectAll').live('click', function () {
      var selectAll = jQuery(this),
          checkboxes = selectAll.closest('table').find('tr td input[type=checkbox]');
      checkboxes.attr('checked', selectAll.is(':checked'));
      if (selectAll.is(':checked')) {
        jQuery('#teaTools input').attr('disabled', false);
      } else {
        jQuery('#teaTools input').attr('disabled', true);
      }
    });

    jQuery(".btnDeleteOrders").live('click', function () {
      if (!confirm('<%= TeaCommerce.Umbraco.Application.Resources.CommonTerms.ConfirmDelete %>')) {
        return false;
      }
    });

      jQuery(".btnCapturePayments").live('click', function () {
        if (!confirm('<%= TeaCommerce.Umbraco.Application.Resources.PaymentProviderTerms.ConfirmPaymentCapture %>')) {
          return false;
        }
      });

  </script>
</asp:Content>
