using TeaCommerce.Api.Services;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public class EmailTemplateTask : ADeleteTask {

    public override bool Delete( long storeId, long entityId ) {
      EmailTemplate emailTemplate = EmailTemplateService.Instance.Get( storeId, entityId );
      return emailTemplate.Delete();
    }

  }
}