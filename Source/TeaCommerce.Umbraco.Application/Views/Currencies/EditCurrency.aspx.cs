using System;
using System.Collections.Generic;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.Currencies {
  public partial class EditCurrency : UmbracoProtectedPage {

    private Currency currency = CurrencyService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, currency.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnCommon, SaveButton_Clicked );
      AddTab( CommonTerms.AvailableInTheseCountries, PnCountries, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlIsoCode.Text = CommonTerms.IsoCode;
      PPnlPriceField.Text = CommonTerms.PricePropertyAlias;
      PPnlCulture.Text = CommonTerms.CultureName;
      PPnlSpecialSymbol.Text = CommonTerms.UseSpecificSymbol;
      PPnlSymbol.Text = CommonTerms.Symbol + "<br /><small>" + CommonTerms.EG + " " + CommonTerms.USD + "</small>";
      PPnlSymbolPlacement.Text = CommonTerms.SymbolPlacement;

      ChkSelectAll.Text = CommonTerms.SelectAll;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {

        CultureCodeSelectorControl.LoadCultureCodes();

        foreach ( Country country in CountryService.Instance.GetAll( currency.StoreId ) ) {
          ChkLstCountries.Items.Add( new ListItem( country.Name, country.Id.ToString(), currency.Id != country.DefaultCurrencyId ) { Selected = currency.AllowedInFollowingCountries.Contains( country.Id ) } );
        }

        TxtName.Text = currency.Name;
        TxtIsoCode.Text = currency.IsoCode;
        TxtPriceField.Text = currency.PricePropertyAlias;

        ChkSpecialSymbol.Checked = !string.IsNullOrEmpty( currency.Symbol );
        TxtSymbol.Text = currency.Symbol;
        RdbListSymbolPlacement.Items.Add( new ListItem( CommonTerms.Left, ( (int)CurrencySymbolPlacement.Left ).ToString() ) );
        RdbListSymbolPlacement.Items.Add( new ListItem( CommonTerms.Right, ( (int)CurrencySymbolPlacement.Right ).ToString() ) );
        RdbListSymbolPlacement.SelectedValue = currency.SymbolPlacement != null ? ( (int?)currency.SymbolPlacement ).ToString() : "0";

        CultureCodeSelectorControl.SelectedValue = currency.CultureName;
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        currency.Name = TxtName.Text;
        currency.IsoCode = TxtIsoCode.Text;
        currency.PricePropertyAlias = TxtPriceField.Text;
        currency.CultureName = CultureCodeSelectorControl.SelectedValue;
        currency.Symbol = ChkSpecialSymbol.Checked ? TxtSymbol.Text : null;
        currency.SymbolPlacement = ChkSpecialSymbol.Checked ? (CurrencySymbolPlacement?)Enum.Parse( typeof( CurrencySymbolPlacement ), RdbListSymbolPlacement.SelectedValue ) : null;
        List<long> allowedRegions = new List<long>();
        foreach ( ListItem listItem in ChkLstCountries.Items ) {
          if ( listItem.Selected ) {
            allowedRegions.Add( long.Parse( listItem.Value ) );
          }
        }
        currency.AllowedInFollowingCountries = allowedRegions;

        currency.Save();
        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.CurrencySaved, string.Empty );
      }
    }

  }
}