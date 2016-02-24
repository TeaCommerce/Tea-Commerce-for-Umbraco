using System;
using System.Collections.Generic;
using System.Linq;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Models.DefaultData;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;

namespace TeaCommerce.Umbraco.Application.Views.CreateAll {
  public partial class CreateAll : UmbracoProtectedPage {
    public long StoreId { get { return long.Parse( Request.QueryString[ "nodeId" ].Split( '_' )[ 1 ] ); } }

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );
      LitCreateAllWarning.Text = CommonTerms.ConfirmCreateAllCountries;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
      LitDefaultCurrency.Text = CommonTerms.DefaultCurrency;
      LitCreateAllComplete.Text = CommonTerms.CreateAllCountriesCompleted;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        DrpCurrencies.DataSource = CurrencyService.Instance.GetAll( StoreId );
        DrpCurrencies.DataBind();
      }
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        List<Country> countries = CountryService.Instance.GetAll( StoreId ).ToList();

        foreach (DefaultCountry defaultCountry in CountryService.Instance.GetDefaultCountries()) {
          if ( !countries.Any(c => c.RegionCode == defaultCountry.Code || c.Name == defaultCountry.Name)) {
            Country country = new Country( StoreId, defaultCountry.Name, long.Parse( DrpCurrencies.SelectedValue ) ) {
              RegionCode = defaultCountry.Code
            };

            country.Save();

            foreach ( Region region in defaultCountry.Regions ) {
              CountryRegion countryRegion = new CountryRegion( StoreId, region.Name, country.Id ) {
                RegionCode = region.Code
              };
              countryRegion.Save();
            }
          }
        }

        CreateControls.Visible = false;
        CreateAllCompleted.Visible = true;
      }
    }
  }
}