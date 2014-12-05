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

namespace TeaCommerce.Umbraco.Application.Views.VATGroups {
  public partial class EditVatGroup : UmbracoProtectedPage {

    private VatGroup vatGroup = VatGroupService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );
    private IEnumerable<CountryRegion> allCountryRegions;

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, vatGroup.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnCommon, SaveButton_Clicked );
      AddTab( CommonTerms.CountrySpecificVat, PnContries, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlVAT.Text = CommonTerms.VatRate;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        TxtName.Text = vatGroup.Name;
        TxtVAT.Text = ( vatGroup.DefaultVatRate * 100M ).ToString( "0.######" );

        allCountryRegions = CountryRegionService.Instance.GetAll( vatGroup.StoreId );

        LvCountrySpecificVatRates.DataSource = ( from r in CountryService.Instance.GetAll( vatGroup.StoreId )
                                                 join vat in vatGroup.CountrySpecificVatRates on r.Id equals vat.Key into rVats
                                                 let vat = rVats.SingleOrDefault()
                                                 select new {
                                                   r.Id,
                                                   r.Name,
                                                   VatRate = ( default( KeyValuePair<long, VatRate> ).Equals( vat ) ? "" : ( vat.Value * 100M ).ToString( "0.######" ) )
                                                 } );
        LvCountrySpecificVatRates.DataBind();
      }
    }

    protected void LvCountrySpecificVatRates_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      long countryId = (long)LvCountrySpecificVatRates.DataKeys[ e.Item.DataItemIndex ][ "Id" ];

      ListView lvCountryRegionSpecificVatRates = e.Item.FindControl<ListView>( "LvCountryRegionSpecificVatRates" );
      lvCountryRegionSpecificVatRates.DataSource = ( from cr in allCountryRegions.Where( cr => cr.CountryId == countryId )
                                                     join vat in vatGroup.CountryRegionSpecificVatRates on cr.Id equals vat.Key into rVats
                                                     let vat = rVats.SingleOrDefault()
                                                     select new {
                                                       Id = cr.Id,
                                                       Name = "&nbsp;&nbsp;&nbsp;" + cr.Name,
                                                       VatRate = ( default( KeyValuePair<long, VatRate> ).Equals( vat ) ? "" : ( vat.Value * 100M ).ToString( "0.######" ) )
                                                     } );
      lvCountryRegionSpecificVatRates.DataBind();
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        vatGroup.Name = TxtName.Text;
        vatGroup.DefaultVatRate = new VatRate( ( TxtVAT.Text.ParseToDecimal() ?? 0M ) / 100M );

        vatGroup.CountrySpecificVatRates.Clear();
        vatGroup.CountryRegionSpecificVatRates.Clear();

        foreach ( ListViewDataItem item in LvCountrySpecificVatRates.Items ) {
          decimal? vatRate = ( item.FindControl<TextBox>( "TxtValue" ) ).Text.ParseToDecimal();
          if ( vatRate != null ) {
            vatGroup.CountrySpecificVatRates.Add( long.Parse( ( item.FindControl<HiddenField>( "HdfKey" ) ).Value ), new VatRate( vatRate.Value / 100M ) );
          }

          //Get country region specific vat rates
          foreach ( ListViewDataItem item2 in item.FindControl<ListView>( "LvCountryRegionSpecificVatRates" ).Items ) {
            decimal? vatRate2 = ( item2.FindControl<TextBox>( "TxtValue" ) ).Text.ParseToDecimal();
            if ( vatRate2 != null ) {
              vatGroup.CountryRegionSpecificVatRates.Add( long.Parse( ( item2.FindControl<HiddenField>( "HdfKey" ) ).Value ), new VatRate( vatRate2.Value / 100M ) );
            }

          }
        }

        vatGroup.Save();
        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.VatGroupSaved, string.Empty );
      }
    }

  }
}