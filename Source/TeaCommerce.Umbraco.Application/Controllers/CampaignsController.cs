using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Marketing.Models.Awards;
using TeaCommerce.Api.Marketing.Models.Rules;
using TeaCommerce.Api.Marketing.Services;
using TeaCommerce.Umbraco.Configuration.Marketing.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using System.Linq;
using TeaCommerce.Api.Marketing.Serialization;
using TeaCommerce.Umbraco.Configuration.Marketing.Serialization;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class CampaignsController : UmbracoAuthorizedApiController {

    [HttpGet]
    public HttpResponseMessage GetManifestsForRules() {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( ManifestService.Instance.GetAllForRules().ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetManifestsForAwards() {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( ManifestService.Instance.GetAllForAwards().ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long campaignId ) {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( CampaignService.Instance.Get( storeId, campaignId ).ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    public class AddRuleGroupPostData {
      public long StoreId { get; set; }
      public long CampaignId { get; set; }
    }

    [HttpPost]
    public HttpResponseMessage AddRuleGroup( AddRuleGroupPostData postData ) {
      Campaign campaign = CampaignService.Instance.Get( postData.StoreId, postData.CampaignId );
      if ( campaign == null ) return null;

      RuleGroup ruleGroup = new RuleGroup { Id = Guid.NewGuid() };

      campaign.RulesGroups.Add( ruleGroup );

      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( ruleGroup.ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    public class AddRulePostData {
      public long StoreId { get; set; }
      public long CampaignId { get; set; }
      public Guid RuleGroupId { get; set; }
      public string RuleAlias { get; set; }
    }

    [HttpPost]
    public HttpResponseMessage AddRule( AddRulePostData postData ) {
      Campaign campaign = CampaignService.Instance.Get( postData.StoreId, postData.CampaignId );
      if ( campaign == null ) return null;

      RuleGroup ruleGroup = campaign.RulesGroups.SingleOrDefault( rg => rg.Id == postData.RuleGroupId );
      if ( ruleGroup == null ) return null;

      IRule rule = RuleService.Instance.Get( postData.RuleAlias );
      if ( rule == null ) return null;

      ruleGroup.Rules.Add( rule );
      campaign.Save();

      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( rule.ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    public class SaveRulePostData {
      public long StoreId { get; set; }
      public long CampaignId { get; set; }
      public long RuleId { get; set; }
      public string Settings { get; set; }
    }

    [HttpPost]
    public void SaveRule( SaveRulePostData postData ) {
      Campaign campaign = CampaignService.Instance.Get( postData.StoreId, postData.CampaignId );
      if ( campaign == null ) return;

      IRule rule = campaign.RulesGroups.SelectMany( rg => rg.Rules ).SingleOrDefault( i => i.Id == postData.RuleId );
      if ( rule == null ) return;

      rule.Settings = postData.Settings;
      rule.SaveSettings();
    }

    public class DeleteRulePostData {
      public long StoreId { get; set; }
      public long CampaignId { get; set; }
      public long RuleId { get; set; }
    }

    [HttpPost]
    public void RemoveRule( DeleteRulePostData postData ) {
      Campaign campaign = CampaignService.Instance.Get( postData.StoreId, postData.CampaignId );
      if ( campaign == null ) return;

      foreach ( RuleGroup ruleGroup in campaign.RulesGroups ) {
        if ( ruleGroup.Rules.RemoveAll( r => r.Id == postData.RuleId ) > 0 ) {
          break;
        }
      }
      campaign.Save();
    }

    public class AddAwardPostData {
      public long StoreId { get; set; }
      public long CampaignId { get; set; }
      public string AwardAlias { get; set; }
    }

    [HttpPost]
    public HttpResponseMessage AddAward( AddAwardPostData postData ) {
      Campaign campaign = CampaignService.Instance.Get( postData.StoreId, postData.CampaignId );
      if ( campaign == null ) return null;

      IAward award = AwardService.Instance.Get( postData.AwardAlias );
      if ( award == null ) return null;

      campaign.Awards.Add( award );
      campaign.Save();

      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( award.ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

    public class SaveAwardPostData {
      public long StoreId { get; set; }
      public long CampaignId { get; set; }
      public long AwardId { get; set; }
      public string Settings { get; set; }
    }

    [HttpPost]
    public void SaveAward( SaveAwardPostData postData ) {
      Campaign campaign = CampaignService.Instance.Get( postData.StoreId, postData.CampaignId );
      if ( campaign == null ) return;

      IAward award = campaign.Awards.SingleOrDefault( i => i.Id == postData.AwardId );
      if ( award == null ) return;

      award.Settings = postData.Settings;
      award.SaveSettings();
    }

    public class DeleteAwardPostData {
      public long StoreId { get; set; }
      public long CampaignId { get; set; }
      public long AwardId { get; set; }
    }

    [HttpPost]
    public void RemoveAward( DeleteAwardPostData postData ) {
      Campaign campaign = CampaignService.Instance.Get( postData.StoreId, postData.CampaignId );
      if ( campaign == null ) return;
      campaign.Awards.RemoveAll( i => i.Id == postData.AwardId );
      campaign.Save();
    }

  }
}