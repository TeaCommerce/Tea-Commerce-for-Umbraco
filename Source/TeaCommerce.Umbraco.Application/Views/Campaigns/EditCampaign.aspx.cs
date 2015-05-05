using System;
using System.Linq;
using System.Security;
using System.Web;
using TeaCommerce.Api.Infrastructure.Licensing;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Marketing.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Configuration.Marketing.Services;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.Campaigns {
  public partial class EditCampaign : UmbracoProtectedPage {

    private Campaign campaign = CampaignService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessMarketing, campaign.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion
      
      AddTab( CommonTerms.Common, PnlCommon, SaveButton_Clicked );

      LitTrialMode.Text = CommonTerms.TrialModeMarketing;
      HypTrialMode.Text = CommonTerms.TrialModeBuy;
      PPnlName.Text = CommonTerms.Name;
      PPnlStartDate.Text = CommonTerms.StartDate;
      PPnlEndDate.Text = CommonTerms.EndDate;
      PPnlIsActive.Text = CommonTerms.Active;
      PPnlAllowAdditionalCampaigns.Text = CommonTerms.AllowAdditionalCampaigns;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        LitScripts.Text = string.Join( "", ManifestService.Instance.GetAllForRules().Concat( ManifestService.Instance.GetAllForAwards() ).SelectMany( i => i.JavaScripts ).Select( i => @"<script src=""" + i + @"""></script>" ) );

        PnlLicenseCheck.Visible = !LicenseService.Instance.ValidateLicenseFeatures( Feature.Marketing );

        TxtName.Text = campaign.Name;
        DPStartDate.DateTime = campaign.StartDate ?? DateTime.MinValue;
        DPEndDate.DateTime = campaign.EndDate != null ? campaign.EndDate.Value : DateTime.MinValue;
        ChkIsActive.Checked = campaign.IsActive;
        ChkAllowAdditionalCampaigns.Checked = campaign.AllowAdditionalCampaigns;
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        campaign.Name = TxtName.Text;
        campaign.StartDate = DPStartDate.DateTime != DateTime.MinValue ? (DateTime?)DPStartDate.DateTime : null;
        campaign.EndDate = DPEndDate.DateTime != DateTime.MinValue ? (DateTime?)DPEndDate.DateTime : null;
        campaign.IsActive = ChkIsActive.Checked;
        campaign.AllowAdditionalCampaigns = ChkAllowAdditionalCampaigns.Checked;
        campaign.Save();
        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.CampaignSave, string.Empty );
      }
    }

  }
}