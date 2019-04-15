using System;
using System.Globalization;
using System.Web.UI;
using TeaCommerce.Umbraco.Application.Resources;
using Umbraco.Web;
using Umbraco.Web.UI.Pages;

namespace TeaCommerce.Umbraco.Application.UI {
  public abstract class UmbracoUserControl : UserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );
      CommonTerms.Culture = new CultureInfo( UmbracoContext.Current.UmbracoUser.Language );
      DeveloperTerms.Culture = new CultureInfo( UmbracoContext.Current.UmbracoUser.Language );
      PaymentProviderTerms.Culture = new CultureInfo( UmbracoContext.Current.UmbracoUser.Language );
      StoreTerms.Culture = new CultureInfo( UmbracoContext.Current.UmbracoUser.Language );
    }

    protected void Redirect( string url) {
      new ClientTools( Page ).ChangeContentFrameUrl( url ).CloseModalWindow();
    }

  }

}