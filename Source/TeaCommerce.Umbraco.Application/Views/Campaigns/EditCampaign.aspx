<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="EditCampaign.aspx.cs"
  Inherits="TeaCommerce.Umbraco.Application.Views.Campaigns.EditCampaign"
  MasterPageFile="../Shared/UmbracoTabView.Master" %>

<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbUIControls" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols.DatePicker" TagPrefix="umbUIControlsDatePicker" %>
<asp:Content ContentPlaceHolderID="CphBody" runat="server">
  <asp:Panel ID="PnlCommon" runat="server">
    <asp:Panel ID="PnlLicenseCheck" runat="server" CssClass="licenseCheck">
      <asp:Literal ID="LitTrialMode" runat="server"></asp:Literal>
      <asp:HyperLink ID="HypTrialMode" runat="server" NavigateUrl="http://www.teacommerce.net" Target="_blank"></asp:HyperLink>
    </asp:Panel>
    <umbUIControls:Pane runat="server">
      <umbUIControls:PropertyPanel ID="PPnlName" runat="server">
        <asp:TextBox ID="TxtName" runat="server" CssClass="guiInputText guiInputStandardSize" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlStartDate" runat="server">
        <umbUIControlsDatePicker:DateTimePicker ID="DPStartDate" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlEndDate" runat="server">
        <umbUIControlsDatePicker:DateTimePicker ID="DPEndDate" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlIsActive" runat="server">
        <asp:CheckBox ID="ChkIsActive" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlAllowAdditionalCampaigns" runat="server">
        <asp:CheckBox ID="ChkAllowAdditionalCampaigns" runat="server" />
      </umbUIControls:PropertyPanel>
      <umbUIControls:PropertyPanel ID="PPnlAllowWithPreviousCampaigns" runat="server">
        <asp:CheckBox ID="ChkAllowWithPreviousCampaigns" runat="server" />
      </umbUIControls:PropertyPanel>
    </umbUIControls:Pane>
    <div ng-app="TeaCommerce" id="marketing" class="umb-pane">
      <div ng-controller="CampaignController">
        <table style="width: 100%">
          <tr>
            <th style="width: 50%; text-align: left;">Rules - <a href="#" ng-click="addRuleGroup()" class="add-rule-group"><i class="icon-add"></i>Add rule group</a></th>
            <th style="width: 50%; text-align: left;">Awards</th>
          </tr>
          <tr>
            <td valign="top">
              <div ng-repeat="ruleGroup in campaign.ruleGroups">
                <div class="andSplit" ng-show="$index > 0">And</div>
                <div class="ruleGroupContent">
                  <select ng-model="ruleGroup.selectedRuleManifest" ng-options="v.name for (k,v) in ruleManifests">
                    <option value="">Choose a rule</option>
                  </select>
                  <a href="" ng-click="addRule(ruleGroup)" class="add-rule"><i class="icon-add"></i>Add rule</a>
                  <div class="orSplit"></div>
                  <div ng-repeat="rule in ruleGroup.rules">
                    <div class="orSplit" ng-show="$index > 0">
                      <span>Or</span>
                    </div>
                    <div class="ruleContent">
                      <strong>{{ruleManifests[rule.ruleAlias].name}}</strong>
                      <ng-include src="ruleManifests[rule.ruleAlias].editor.view"></ng-include>
                    </div>
                  </div>
                </div>
              </div>
            </td>
            <td valign="top">
              <div class="ruleGroupContent">
                <select ng-model="selectedAwardManifest" ng-options="v.name for (k,v) in awardManifests">
                  <option value="">Choose an award</option>
                </select>
                <a href="" ng-click="addAward()" class="add-award"><i class="icon-add"></i>Add award</a>
                <div class="orSplit"></div>
                <div ng-repeat="award in campaign.awards">
                  <div class="orSplit" ng-show="$index > 0">
                    <span>And</span>
                  </div>
                  <div class="awardContent">
                    <strong>{{awardManifests[award.awardAlias].name}}</strong>
                    <ng-include src="awardManifests[award.awardAlias].editor.view"></ng-include>
                  </div>
                </div>
              </div>
            </td>
          </tr>
        </table>
      </div>
    </div>
  </asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="CphHead">
  <script src="//ajax.googleapis.com/ajax/libs/angularjs/1.2.21/angular.min.js"></script>
  <script src="/App_Plugins/TeaCommerce/Assets/Scripts/tea-commerce.min.js"></script>
  <script src="/App_Plugins/TeaCommerce/Marketing/Assets/Scripts/campaign.controller.js"></script>

  <script type="text/javascript">
    function openContentPicker(fn) {
      parent.UmbClientMgr.openModalWindow('<%: TreeUrlGenerator.GetPickerUrl( "content", "content" ) %>', 'Choose Content', true, 300, 400, 60, null, null, function (args) {
        if (fn && args && args.outVal) {
          findContentName(args.outVal, function (name, breadcrump) {
            fn(args.outVal, name, breadcrump);
          });
        }
      });
    }

    function findContentName(nodeId, fn) {
      if (nodeId) {
        $.ajax({
          type: "POST",
          url: '/umbraco/webservices/legacyAjaxCalls.asmx/GetNodeBreadcrumbs',
          data: '{ "nodeId": ' + nodeId + ' }',
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (msg) {
            var a = msg.d;
            var name = a[a.length - 1];
            var breadcrumb = a.join(" > ");

            if (fn) {
              fn(name, breadcrumb);
            }
          }
        });
      }
    }
  </script>

  <asp:Literal runat="server" ID="LitScripts"></asp:Literal>

  <style type="text/css">
    #marketing .table { margin-bottom: 0; }
    #marketing .table td { border-top: none; }
    #marketing input, #marketing select { width: auto; }
    #marketing label { display: inline; margin-right: 5px; padding-left: 3px; }
    .licenseCheck { background: #FFF6BF; margin: 10px 0; padding: 20px; color: #5E532C; text-align: center; }
    div.ruleGroupContent { border: 1px solid #ddd; border-radius: 4px 4px 0 0; padding: 10px; }
    div.andSplit { margin: 10px 15px; font-weight: bold; }
    div.orSplit { position: relative; margin: 15px 0; border-top: 1px solid #ddd; }
    div.orSplit span { position: absolute; top: -7px; left: 15px; background: #FFF; padding: 0 10px; font-weight: bold; }
    span.accumulate { display: inline-block; padding: 4px 5px 4px 10px; }
    table.table td { padding-top: 3px; padding-bottom: 3px; }
    table.table.currencies { margin-top: 3px; }
    table.table.currencies td { padding-left: 0; }
    td.properties input, td.properties select { margin-bottom: 6px; }
    td.properties span.divider { display: inline-block; padding: 3px 0px; }
    table.table td { padding-top: 4px; }
    td.buttons a { float: left; clear: both; }
    td.buttons a i[class^="icon-"], td.buttons a i[class*=" icon-"],
    a.add-rule-group i[class^="icon-"], a.add-rule i[class^="icon-"], a.add-award i[class^="icon-"],
    a.add-discount-code-rule i[class^="icon-"], a.choose-product i[class^="icon-"],
    a.add-rule-group i[class*=" icon-"], a.add-rule i[class*=" icon-"], a.add-award i[class*=" icon-"],
    a.add-discount-code-rule i[class*=" icon-"], a.choose-product i[class*=" icon-"] { text-decoration: none; padding-right: 3px; }
    a.add-rule-group { font-weight: normal; }
    a.add-rule, a.add-award { position: relative; top: 5px; font-weight: normal; }
    label.no-edit { cursor: text; }
    .umbDateTimePicker button { background-image: url(<%= TeaCommerce.Umbraco.Application.Utils.WebUtils.GetWebResourceUrl(TeaCommerce.Umbraco.Application.Constants.EditorIcons.Calendar) %>) !important;}
  </style>
</asp:Content>
