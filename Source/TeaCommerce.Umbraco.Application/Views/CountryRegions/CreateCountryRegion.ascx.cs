using System;
using System.Web;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.CountryRegions {
  public partial class CreateCountryRegion : StoreSpecificUserControl {

    private long _countryId = long.Parse( HttpContext.Current.Request.QueryString[ "nodeId" ].Split( '_' )[ 2 ] );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        CountryRegion countryRegion = new CountryRegion( StoreId, TxtName.Text, _countryId );
        countryRegion.Save();

        Redirect( WebUtils.GetPageUrl( Constants.Pages.EditCountryRegion ) + "?id=" + countryRegion.Id + "&storeId=" + countryRegion.StoreId + "&countryId=" + _countryId );
      }

    }

  }
}