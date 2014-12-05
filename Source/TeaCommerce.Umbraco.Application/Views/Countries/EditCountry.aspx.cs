using System;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.Countries {
  public partial class EditCountry : UmbracoProtectedPage {

    private Country country = CountryService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, country.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnCommon, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlRegionCode.Text = CommonTerms.RegionCode;
      PPnlStandardCurrency.Text = CommonTerms.DefaultCurrency;
      PPnlStandardShippingMethod.Text = CommonTerms.DefaultShippingMethod;
      PPnlStandardPaymentMethod.Text = CommonTerms.DefaultPaymentMethod;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {

        //Load currencies
        DrpCurrencies.DataSource = CurrencyService.Instance.GetAll( country.StoreId );
        DrpCurrencies.DataBind();

        LoadShippingAndPaymentMethods( country.Id );

        TxtName.Text = country.Name;
        TxtRegionCode.Text = country.RegionCode;
        DrpCurrencies.SelectedValue = country.DefaultCurrencyId.ToString();

        if ( country.DefaultShippingMethodId != null )
          DrpShippingMethods.SelectedValue = country.DefaultShippingMethodId.Value.ToString();
        if ( country.DefaultPaymentMethodId != null )
          DrpPaymentMethods.SelectedValue = country.DefaultPaymentMethodId.Value.ToString();
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {

        country.Name = TxtName.Text;
        country.RegionCode = TxtRegionCode.Text;
        country.DefaultCurrencyId = long.Parse( DrpCurrencies.SelectedValue );
        country.DefaultShippingMethodId = DrpShippingMethods.SelectedValue.TryParse<long>();
        country.DefaultPaymentMethodId = DrpPaymentMethods.SelectedValue.TryParse<long>();

        country.Save();

        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.CountrySaved, string.Empty );
      }
    }

    private void LoadShippingAndPaymentMethods( long countryId ) {
      DrpShippingMethods.Items.Clear();
      DrpShippingMethods.Items.Add( new ListItem( "----", string.Empty ) );
      DrpShippingMethods.DataSource = ShippingMethodService.Instance.GetAll( country.StoreId );
      DrpShippingMethods.DataBind();

      DrpPaymentMethods.Items.Clear();
      DrpPaymentMethods.Items.Add( new ListItem( "----", string.Empty ) );
      DrpPaymentMethods.DataSource = PaymentMethodService.Instance.GetAll( country.StoreId );
      DrpPaymentMethods.DataBind();
    }

  }
}