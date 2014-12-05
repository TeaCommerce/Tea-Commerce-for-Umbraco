<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditShippingMethod.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.ShippingMethods.EditShippingMethod"
  MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<%@ Register Assembly="umbraco" Namespace="umbraco.controls" TagPrefix="umbControls" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlCommon" runat="server">
    <umbUIControls:Pane runat="server">
      <umbUIControls:PropertyPanel ID="PPnlName" runat="server">
        <asp:TextBox ID="TxtName" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlDictionaryItemName" runat="server">
        <asp:TextBox ID="TxtDictionaryItemName" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlSku" runat="server">
        <asp:TextBox ID="TxtSku" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlVatGroup" runat="server">
        <asp:DropDownList ID="DrpVatGroups" runat="server" DataTextField="Name" DataValueField="Id"
          CssClass="guiInputText guiInputStandardSize" AppendDataBoundItems="true">
          <asp:ListItem Text="----" Value="" />
        </asp:DropDownList>
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlImage" runat="server">
        <umbControls:ContentPicker ID="CPImage" runat="server" AppAlias="media" TreeAlias="media" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
    <umbUIControls:Pane ID="PnDefaultCurrencies" runat="server">
      <asp:ListView ID="LvDefaultCurrencies" runat="server" OnItemDataBound="LvDefaultCurrencies_ItemDataBound">
        <LayoutTemplate>
          <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
          <div class="umb-el-wrap">
            <asp:HiddenField ID="HdfId" runat="server" Value='<%# Eval("Id") %>' />
            <asp:HiddenField ID="HdfPriceId" runat="server" />
            <label class="control-label">
              <asp:Literal ID="LitHeader" runat="server" Text='<%# Eval("Name") %>'></asp:Literal></label>
            <div class="controls controls-row">
              <asp:TextBox ID="TxtPrice" runat="server" CssClass="guiInputText guiInputStandardSize" />
            </div>
          </div>
        </ItemTemplate>
      </asp:ListView>
    </umbUIControls:Pane>
  </asp:Panel>
  <umbUIControls:Pane ID="PnCurrencies" runat="server">
    <div class="umb-el-wrap hidelabel">
      <div class="controls controls-row">
        <asp:CheckBox runat="server" ID="ChkSelectAll" CssClass="selectAll" />
        <div class="countries checkboxesForSelectAll">
          <asp:ListView ID="LvCountries" runat="server" OnItemDataBound="LvCountries_ItemDataBound">
            <LayoutTemplate>
              <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
            </LayoutTemplate>
            <ItemTemplate>
              <div style="margin: 1px 1px 1px 1px;">
                <asp:HiddenField ID="HdfId" runat="server" Value='<%# Eval("Id") %>' />
                <asp:CheckBox ID="ChkIsSupported" runat="server" /><asp:Label runat="server" Text='<%# Eval( "Name" ) %>' AssociatedControlID="ChkIsSupported" /><span class="customPrices">(<asp:HyperLink ID="HypCustomPrices" runat="server" NavigateUrl="#" />)</span>
              </div>
              <div class="propertypane pnlCurrencies">
                <asp:ListView ID="LvCurrencies" runat="server" OnLayoutCreated="LvCurrencies_LayoutCreated"
                  OnItemDataBound="LvCurrencies_ItemDataBound">
                  <LayoutTemplate>
                    <div class="umb-el-wrap">
                      <label class="control-label">
                        <asp:Literal ID="LitCurrency" runat="server" /></label>
                      <div class="controls controls-row">
                        <asp:Literal ID="LitShippingFee" runat="server" />
                      </div>
                    </div>
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                  </LayoutTemplate>
                  <ItemTemplate>
                    <div class="umb-el-wrap">
                      <asp:HiddenField ID="HdfId" runat="server" Value='<%# Eval("Id") %>' />
                      <asp:HiddenField ID="HdfPriceId" runat="server" />
                      <asp:Label ID="LblCurrencyName" runat="server" CssClass="control-label" AssociatedControlID="TxtPrice" />
                      <div class="controls controls-row">
                        <asp:TextBox ID="TxtPrice" CssClass="guiInputText" runat="server" />
                      </div>
                    </div>
                  </ItemTemplate>
                </asp:ListView>
              </div>
              <asp:ListView ID="LvCountryRegions" runat="server" OnItemDataBound="LvCountryRegions_ItemDataBound">
                <LayoutTemplate>
                  <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                  <div style="margin: 1px 1px 1px 20px;">
                    <asp:HiddenField ID="HdfId" runat="server" Value='<%# Eval("Id") %>' />
                    <asp:CheckBox ID="ChkIsSupported" runat="server" /><asp:Label runat="server" Text='<%# Eval( "Name" ) %>' AssociatedControlID="ChkIsSupported" /><span class="customPrices">(<asp:HyperLink ID="HypCustomPrices" runat="server" NavigateUrl="#" />)</span>
                  </div>
                  <div class="propertypane pnlCurrencies" style="margin-left: 20px;">
                    <asp:ListView ID="LvCurrencies" runat="server" OnLayoutCreated="LvCurrencies_LayoutCreated"
                      OnItemDataBound="LvCurrencies_ItemDataBound">
                      <LayoutTemplate>
                        <div class="umb-el-wrap">
                          <label class="control-label">
                            <asp:Literal ID="LitCurrency" runat="server" /></label>
                          <div class="controls controls-row">
                            <asp:Literal ID="LitShippingFee" runat="server" />
                          </div>
                        </div>
                        <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                      </LayoutTemplate>
                      <ItemTemplate>
                        <div class="umb-el-wrap">
                          <asp:HiddenField ID="HdfId" runat="server" Value='<%# Eval("Id") %>' />
                          <asp:HiddenField ID="HdfPriceId" runat="server" />
                          <asp:Label ID="LblCurrencyName" runat="server" CssClass="control-label" AssociatedControlID="TxtPrice" />
                          <div class="controls controls-row">
                            <asp:TextBox ID="TxtPrice" CssClass="guiInputText" runat="server" />
                          </div>
                        </div>
                      </ItemTemplate>
                    </asp:ListView>
                  </div>
                </ItemTemplate>
              </asp:ListView>
            </ItemTemplate>
          </asp:ListView>
        </div>
      </div>
    </div>
  </umbUIControls:Pane>
</asp:Content>
<asp:Content ContentPlaceHolderID="CphHead" runat="server">
  <style type="text/css">
    .hide {
      display: none;
    }

    .pnlCurrencies {
      margin-top: 2px;
      margin-bottom: 5px;
    }

    .customPrices {
      font-size: 10px;
      padding-left: 5px;
    }

    .selectAll {
      font-size: 11px;
      display: inline-block;
      padding-left: 3px;
      margin-bottom: 5px;
    }
  </style>
  <script type="text/javascript" src='<%= TeaCommerce.Umbraco.Application.Utils.WebUtils.GetWebResourceUrl(TeaCommerce.Umbraco.Application.Constants.Scripts.Default) %>'></script>
</asp:Content>
