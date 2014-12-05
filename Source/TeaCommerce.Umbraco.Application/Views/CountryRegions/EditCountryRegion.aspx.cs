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

namespace TeaCommerce.Umbraco.Application.Views.CountryRegions {
  public partial class EditCountryRegion : UmbracoProtectedPage {

    private CountryRegion countryRegion = CountryRegionService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, countryRegion.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnCommon, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlRegionCode.Text = CommonTerms.RegionCode;
      PPnlStandardShippingMethod.Text = CommonTerms.DefaultShippingMethod;
      PPnlStandardPaymentMethod.Text = CommonTerms.DefaultPaymentMethod;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {

        LoadShippingAndPaymentMethods( countryRegion.Id );

        TxtName.Text = countryRegion.Name;
        TxtRegionCode.Text = countryRegion.RegionCode;

        if ( countryRegion.DefaultShippingMethodId != null )
          DrpShippingMethods.SelectedValue = countryRegion.DefaultShippingMethodId.Value.ToString();
        if ( countryRegion.DefaultPaymentMethodId != null )
          DrpPaymentMethods.SelectedValue = countryRegion.DefaultPaymentMethodId.Value.ToString();
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {

        countryRegion.Name = TxtName.Text;
        countryRegion.RegionCode = TxtRegionCode.Text;
        countryRegion.DefaultShippingMethodId = DrpShippingMethods.SelectedValue.TryParse<long>();
        countryRegion.DefaultPaymentMethodId = DrpPaymentMethods.SelectedValue.TryParse<long>();

        countryRegion.Save();

        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.CountryRegionSaved, string.Empty );
      }
    }

    private void LoadShippingAndPaymentMethods( long regionId ) {
      DrpShippingMethods.Items.Clear();
      DrpShippingMethods.Items.Add( new ListItem( "----", string.Empty ) );
      DrpShippingMethods.DataSource = ShippingMethodService.Instance.GetAll( countryRegion.StoreId );
      DrpShippingMethods.DataBind();

      DrpPaymentMethods.Items.Clear();
      DrpPaymentMethods.Items.Add( new ListItem( "----", string.Empty ) );
      DrpPaymentMethods.DataSource = PaymentMethodService.Instance.GetAll( countryRegion.StoreId );
      DrpPaymentMethods.DataBind();
    }

  }
}