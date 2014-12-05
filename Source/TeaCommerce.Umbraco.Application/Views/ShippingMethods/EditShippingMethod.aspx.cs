using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TeaCommerce.Umbraco.Application.Views.ShippingMethods {
  public partial class EditShippingMethod : UmbracoProtectedPage {

    private ShippingMethod shippingMethod = ShippingMethodService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );
    private IEnumerable<CountryRegion> allCountryRegions;
    private IEnumerable<Currency> allCurrencies;
    long currentCountryId;
    long? currentCountryRegionId;

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, shippingMethod.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnlCommon, SaveButton_Clicked );
      AddTab( CommonTerms.AvailableInTheseCountries, PnCurrencies, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlDictionaryItemName.Text = CommonTerms.Alias;
      PPnlSku.Text = CommonTerms.Sku;
      PPnlVatGroup.Text = CommonTerms.VatGroup;
      PPnlImage.Text = CommonTerms.Image;
      PnDefaultCurrencies.Text = CommonTerms.DefaultPrices;

      ChkSelectAll.Text = CommonTerms.SelectAll;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {

        DrpVatGroups.DataSource = VatGroupService.Instance.GetAll( shippingMethod.StoreId );
        DrpVatGroups.DataBind();

        TxtName.Text = shippingMethod.Name;
        TxtDictionaryItemName.Text = shippingMethod.Alias;
        TxtSku.Text = shippingMethod.Sku;
        if ( shippingMethod.VatGroupId != null ) {
          DrpVatGroups.SelectedValue = shippingMethod.VatGroupId.Value.ToString();
        }
        if ( !string.IsNullOrEmpty( shippingMethod.ImageIdentifier ) ) {
          CPImage.Value = shippingMethod.ImageIdentifier;
        }

        LoadCurrenciesAndCountries();
      }
    }

    private void LoadCurrenciesAndCountries() {
      allCountryRegions = CountryRegionService.Instance.GetAll( shippingMethod.StoreId );
      allCurrencies = CurrencyService.Instance.GetAll( shippingMethod.StoreId );

      LvDefaultCurrencies.DataSource = allCurrencies;
      LvDefaultCurrencies.DataBind();

      LvCountries.DataSource = CountryService.Instance.GetAll( shippingMethod.StoreId );
      LvCountries.DataBind();
    }

    protected void LvDefaultCurrencies_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      Currency currency = (Currency)( e.Item as ListViewDataItem ).DataItem;

      ServicePrice servicePrice = shippingMethod.OriginalPrices.Get( currency.Id );
      if ( servicePrice != null ) {
        e.Item.FindControl<HiddenField>( "HdfPriceId" ).Value = servicePrice.Id.ToString();
        e.Item.FindControl<TextBox>( "TxtPrice" ).Text = servicePrice.Value.ToString( "0.####" );
      }
    }

    protected void LvCountries_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      Country country = (Country)( e.Item as ListViewDataItem ).DataItem;
      currentCountryId = country.Id;
      currentCountryRegionId = null;

      e.Item.FindControl<CheckBox>( "ChkIsSupported" ).Checked = shippingMethod.AllowedInFollowingCountries.Contains( country.Id );
      e.Item.FindControl<HyperLink>( "HypCustomPrices" ).Text = CommonTerms.CustomPrices;

      ListView LvCurrencies = (ListView)e.Item.FindControl( "LvCurrencies" );
      LvCurrencies.DataSource = allCurrencies.Where( c => c.AllowedInFollowingCountries.Contains( country.Id ) );
      LvCurrencies.DataBind();

      ListView lvCountryRegions = (ListView)e.Item.FindControl( "LvCountryRegions" );
      lvCountryRegions.DataSource = allCountryRegions.Where( cr => cr.CountryId == country.Id );
      lvCountryRegions.DataBind();
    }

    protected void LvCountryRegions_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      CountryRegion countryRegion = (CountryRegion)( e.Item as ListViewDataItem ).DataItem;
      currentCountryRegionId = countryRegion.Id;

      e.Item.FindControl<CheckBox>( "ChkIsSupported" ).Checked = shippingMethod.AllowedInFollowingCountryRegions.Contains( countryRegion.Id );
      e.Item.FindControl<HyperLink>( "HypCustomPrices" ).Text = CommonTerms.CustomPrices;

      ListView LvCurrencies = (ListView)e.Item.FindControl( "LvCurrencies" );
      LvCurrencies.DataSource = allCurrencies.Where( c => c.AllowedInFollowingCountries.Contains( countryRegion.CountryId ) );
      LvCurrencies.DataBind();
    }

    protected void LvCurrencies_LayoutCreated( object sender, EventArgs e ) {
      ListView lvCurrencies = sender as ListView;
      lvCurrencies.FindControl<Literal>( "LitCurrency" ).Text = CommonTerms.Currency;
      lvCurrencies.FindControl<Literal>( "LitShippingFee" ).Text = CommonTerms.Price;
    }

    protected void LvCurrencies_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      Currency currency = (Currency)( e.Item as ListViewDataItem ).DataItem;

      //Write Currency name
      e.Item.FindControl<Label>( "LblCurrencyName" ).Text = currency.Name;

      ServicePrice servicePrice = shippingMethod.OriginalPrices.Get( currency.Id, currentCountryId, currentCountryRegionId );
      if ( servicePrice != null ) {
        e.Item.FindControl<HiddenField>( "HdfPriceId" ).Value = servicePrice.Id.ToString();
        e.Item.FindControl<TextBox>( "TxtPrice" ).Text = servicePrice.Value.ToString( "0.####" );
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        //Get the name and media nodeId
        shippingMethod.Name = TxtName.Text;
        shippingMethod.Alias = TxtDictionaryItemName.Text;
        shippingMethod.Sku = TxtSku.Text;
        shippingMethod.VatGroupId = DrpVatGroups.SelectedValue.TryParse<long>();
        shippingMethod.ImageIdentifier = CPImage.Value;

        shippingMethod.AllowedInFollowingCountries = new List<long>();
        shippingMethod.AllowedInFollowingCountryRegions = new List<long>();

        //Add default prices
        LoopPrices( LvDefaultCurrencies );

        //Run through countries
        foreach ( ListViewDataItem item in LvCountries.Items ) {
          long countryId = long.Parse( item.FindControl<HiddenField>( "HdfId" ).Value );

          //Check to see if the country has been selected
          if ( item.FindControl<CheckBox>( "ChkIsSupported" ).Checked ) {
            shippingMethod.AllowedInFollowingCountries.Add( countryId );
          }

          LoopPrices( item.FindControl<ListView>( "LvCurrencies" ), countryId );

          //Run through country regions
          ListView lvCountryRegions = item.FindControl<ListView>( "LvCountryRegions" );
          foreach ( ListViewDataItem item2 in lvCountryRegions.Items ) {
            long countryRegionId = long.Parse( item2.FindControl<HiddenField>( "HdfId" ).Value );

            //Check to see if the country has been selected
            if ( item2.FindControl<CheckBox>( "ChkIsSupported" ).Checked ) {
              shippingMethod.AllowedInFollowingCountryRegions.Add( countryRegionId );
            }

            LoopPrices( item2.FindControl<ListView>( "LvCurrencies" ), countryId, countryRegionId );
          }

        }

        //Save the shippingmethod
        shippingMethod.Save();

        LoadCurrenciesAndCountries();

        //Show confirming speech bubble in umbraco
        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.ShippingMethodSaved, string.Empty );
      }
    }

    private void LoopPrices( ListView lvCurrencies, long? countryId = null, long? countryRegionId = null ) {
      foreach ( ListViewDataItem item in lvCurrencies.Items ) {
        long currencyId = long.Parse( ( item.FindControl<HiddenField>( "HdfId" ) ).Value );
        long? servicePriceId = item.FindControl<HiddenField>( "HdfPriceId" ).Value.TryParse<long>();
        decimal? price = ( item.FindControl<TextBox>( "TxtPrice" ) ).Text.ParseToDecimal();

        if ( servicePriceId != null ) {
          if ( price != null ) {
            ServicePrice servicePrice = shippingMethod.OriginalPrices.SingleOrDefault( p => p.Id == servicePriceId.Value );
            servicePrice.Value = price.Value;
          } else {
            shippingMethod.OriginalPrices.RemoveAll( p => p.Id == servicePriceId.Value );
          }
        } else {
          if ( price != null ) {
            shippingMethod.OriginalPrices.Add( countryId == null ? new ServicePrice( currencyId, price.Value ) : new ServicePrice( currencyId, price.Value, countryId.Value, countryRegionId ) );
          }
        }

      }

    }

  }
}