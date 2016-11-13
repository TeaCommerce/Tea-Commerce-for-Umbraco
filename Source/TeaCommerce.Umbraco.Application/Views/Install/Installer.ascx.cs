using System;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;

namespace TeaCommerce.Umbraco.Install.Views {
  public partial class Installer : UmbracoUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitComplete.Text = DeveloperTerms.Installation_Complete;
      LitStep1.Text = DeveloperTerms.Installation_StepsToComplete1;
      LitStep2.Text = DeveloperTerms.Installation_StepsToComplete2;

      LitUsefulLinks.Text = DeveloperTerms.UsefulLinks;
      LitBuyALicense.Text = DeveloperTerms.BuyALicense;
      LitDocumentation.Text = DeveloperTerms.Documentation;
      LitSupportForum.Text = DeveloperTerms.SupportForum;
      LitStarterKit.Text = DeveloperTerms.StarterKitPackage;
      LitRevisionHistory.Text = DeveloperTerms.RevisionHistory;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        CodeArea1.Text = @"<script type=""text/javascript"" src=""//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js""></script>
<script type=""text/javascript"" src=""//cdnjs.cloudflare.com/ajax/libs/jquery.form/3.50/jquery.form.min.js""></script>
<script type=""text/javascript"" src=""/App_Plugins/TeaCommerce/Assets/Scripts/tea-commerce.min.js""></script>
<script type=""text/javascript"">
  _storeId = [fetch the store id from Umbraco here];
</script>";
      }
    }

  }
}