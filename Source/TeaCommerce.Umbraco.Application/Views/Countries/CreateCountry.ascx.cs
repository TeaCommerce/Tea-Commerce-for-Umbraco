using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Models.DefaultData;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.Countries {
  public partial class CreateCountry : StoreSpecificUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );
      LitSelectCountry.Text = CommonTerms.SelectCountryFromList;
      LitOrTypeName.Text = CommonTerms.OrTypeName;
      LitDefaultCurrency.Text = CommonTerms.DefaultCurrency;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        DrpCurrencies.DataSource = CurrencyService.Instance.GetAll( StoreId );
        DrpCurrencies.DataBind();

        IEnumerable<DefaultCountry> defaultCountries = CountryService.Instance.GetDefaultCountries();

        DrpCountries.DataSource = defaultCountries;
        DrpCountries.DataBind();
      }
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        Country country;

        if ( !string.IsNullOrEmpty( TxtName.Text ) ) {
          country = new Country( StoreId, TxtName.Text, long.Parse( DrpCurrencies.SelectedValue ) );
          country.Save();
        } else {
          DefaultCountry defaultCountry = CountryService.Instance.GetDefaultCountries().First( c => c.Code == DrpCountries.SelectedValue );

          country = new Country( StoreId, defaultCountry.Name, long.Parse( DrpCurrencies.SelectedValue ) ) {
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
    }
  }
}