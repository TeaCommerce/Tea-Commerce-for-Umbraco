using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Marketing.Services;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class CampaignTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      Campaign campaign = CampaignService.Instance.Get( storeId, entityId );
      return campaign.Delete();
    }

  }
}