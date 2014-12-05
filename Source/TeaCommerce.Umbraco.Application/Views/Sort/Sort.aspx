<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sort.aspx.cs" Inherits="TeaCommerce.Umbraco.Application.Views.Sort.Sort"
  MasterPageFile="~/Umbraco/masterpages/umbracoDialog.Master" %>

<%@ Register TagPrefix="umbUI" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register TagPrefix="CD" Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style type="text/css">
    #sortableFrame { height: 270px; overflow: auto; border: 1px solid #ccc; }
    #sortableNodes { padding: 4px; width: 100%; }
    #sortableNodes thead tr th { border-bottom: 1px solid #ccc; cursor: pointer; padding: 4px; padding-right: 25px; background-image: url(<%= umbraco.IO.IOHelper.ResolveUrl(umbraco.IO.SystemDirectories.Umbraco_client) %>/tableSorting/img/bg.gif); cursor: pointer; font-weight: bold; background-repeat: no-repeat; background-position: center right; }
    #sortableNodes thead tr th#sortSort { width: 60px; }
    #sortableNodes thead tr th#sortName { width: auto; }
    
    #sortableNodes thead tr th.headerSortDown { background-image: url(<%= umbraco.IO.IOHelper.ResolveUrl(umbraco.IO.SystemDirectories.Umbraco_client) %>/tableSorting/img/desc.gif); }
    
    #sortableNodes thead tr th.headerSortUp { background-image: url(<%= umbraco.IO.IOHelper.ResolveUrl(umbraco.IO.SystemDirectories.Umbraco_client) %>/tableSorting/img/asc.gif); }
    
    #sortableNodes tbody tr td { border-bottom: 1px solid #efefef; }
    #sortableNodes td { padding: 4px; cursor: move; }
    tr.tDnD_whileDrag, tr.tDnD_whileDrag td { background: #dcecf3; border-color: #a8d8eb !important; margin-top: 20px; }
    #sortableNodes .nowrap { white-space: nowrap; }
  </style>
  <script type="text/javascript">
    jQuery(document).ready(function () {
      jQuery('#sortableNodes tbody').sortable({
        stop: function (event, ui) {
          updateSorting();
        }
      });
      jQuery("#sortableNodes").tablesorter()
      jQuery("#sortableNodes").bind("sortEnd", function () {
        updateSorting();
      });
    });

    function updateSorting() {
      jQuery('#sortableNodes .sort input').each(function (i) {
        jQuery(this).val(i);
      });
    }

  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  <CD:JsInclude ID="JsInclude1" runat="server" FilePath="tablesorting/tableFilter.js"
    PathNameAlias="UmbracoClient" />
  <CD:JsInclude ID="JsInclude2" runat="server" FilePath="ui/jqueryui.js" PathNameAlias="UmbracoClient" />
  <asp:Panel ID="PnlRefresh" runat="server">
    <umbUI:Pane runat="server" ID="sortPane">
      <p class="help">
        <asp:Literal ID="LitSortHelp" runat="server" />
      </p>
      <asp:ListView ID="LvSortables" runat="server" OnLayoutCreated="LvSortables_LayoutCreated">
        <LayoutTemplate>
          <div id="sortableFrame">
            <table id="sortableNodes" cellspacing="0">
              <thead>
                <tr>
                  <th class="header" id="sortName">
                    <asp:Literal ID="LtNameHeader" runat="server"></asp:Literal>
                  </th>
                  <th class="header" id="sortSort">
                    <asp:Literal ID="LtNameSort" runat="server"></asp:Literal>
                  </th>
                </tr>
              </thead>
              <tbody>
                <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
              </tbody>
            </table>
          </div>
        </LayoutTemplate>
        <ItemTemplate>
          <tr>
            <td>
              <span class="sort">
                <asp:HiddenField ID="HdnSort" Value='<%# Eval("Sort") %>' runat="server" />
              </span>
              <asp:HiddenField ID="HdnId" runat="server" Value='<%# Eval("Id") %>' />
              <%# Eval("Name") %>
            </td>
            <td>
              <%# Eval("Sort") %>
            </td>
          </tr>
        </ItemTemplate>
      </asp:ListView>
    </umbUI:Pane>
    <br />
    <p>
      <asp:Button ID="BtnSubmit" CssClass="btnSubmit" runat="server" OnClick="BtnSubmit_Click" />
      <em>or </em><a href="#" style="color: blue" onclick="UmbClientMgr.closeModalWindow()">
        <asp:Literal ID="LitCancel" runat="server" /></a>
    </p>
  </asp:Panel>
</asp:Content>
