using System;
using System.Globalization;
using System.Web.UI;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Views.Shared;
using Umbraco.Web.UI.Pages;
using umbraco.uicontrols;

namespace TeaCommerce.Umbraco.Application.UI {

  public abstract class UmbracoProtectedPage : UmbracoEnsuredPage {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      string cultureName = UmbracoUser.Language;
      if ( cultureName == "en_us" ) {
        cultureName = "en-us";
      }

      CommonTerms.Culture = new CultureInfo( cultureName );
      DeveloperTerms.Culture = new CultureInfo( cultureName );
      PaymentProviderTerms.Culture = new CultureInfo( cultureName );
      StoreTerms.Culture = new CultureInfo( cultureName );
    }

    public TabView CurrentTabView { get { return ( Master as UmbracoTabView ).CurrentTabView; } }

    protected TabPage AddTab( string name, Control control ) {
      return AddTab( name, control, null );
    }

    protected TabPage AddTab( string name, Control control, EventHandler saveMethod ) {
      TabPage tabPage = CurrentTabView.NewTabPage( name );

      if ( control != null )
        tabPage.Controls.Add( control );
      if ( tabPage.Menu.Icons.Count == 0 ) {
        AddButton( "btn btn-primary btnSave", CommonTerms.Save, tabPage, saveMethod );
      }
      return tabPage;
    }

    protected void AddButton( string cssClass, string text, TabPage tabPage, EventHandler method ) {
      if ( method != null ) {
        MenuButton button = tabPage.Menu.NewButton();
        button.Text = text;
        button.CssClass = cssClass;
        button.Click += method;
      }

    }
  }
}