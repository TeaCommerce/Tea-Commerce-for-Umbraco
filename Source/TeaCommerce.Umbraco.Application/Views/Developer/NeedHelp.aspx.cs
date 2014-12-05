using System;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;

namespace TeaCommerce.Umbraco.Application.Views.Developer {
  public partial class NeedHelp : UmbracoProtectedPage {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      AddTab( CommonTerms.Common, PnlGetStarted );

      LitUsefulLinks.Text = DeveloperTerms.UsefulLinks;
      LitBuyALicense.Text = DeveloperTerms.BuyALicense;
      LitDocumentation.Text = DeveloperTerms.Documentation;
      LitSupportForum.Text = DeveloperTerms.SupportForum;
      LitStarterKit.Text = DeveloperTerms.StarterKitPackage;
      LitRevisionHistory.Text = DeveloperTerms.RevisionHistory;
    }

  }
}