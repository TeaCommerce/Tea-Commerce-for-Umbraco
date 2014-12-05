using System;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.Campaigns {
  public partial class CreateCampaign : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        Campaign campaign = new Campaign( StoreId, TxtName.Text );
        campaign.Save();
        Redirect( WebUtils.GetPageUrl( Constants.Pages.EditCampaign ) + "?id=" + campaign.Id + "&storeId=" + campaign.StoreId );
      }

    }

  }
}