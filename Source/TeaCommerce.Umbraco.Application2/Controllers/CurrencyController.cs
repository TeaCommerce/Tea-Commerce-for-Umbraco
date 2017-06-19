using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Api.Services;
using Umbraco.Web.Editors;
using TeaCommerce.Umbraco.Application2;
using Umbraco.Web.Mvc;
using TeaCommerce.Api.Models;
using System.Globalization;

namespace TeaCommerce.Umbraco.Application2.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class CurrencyController : UmbracoAuthorizedJsonController {

    [HttpGet]
    public HttpResponseMessage Get( long storeId, long currencyId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( CurrencyService.Instance.Get( storeId, currencyId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetCountries( long storeId ) {

      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( CountryService.Instance.GetAll( storeId ).ToJson() )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetCultures( long storeId ) {
      var cultures = from c in CultureInfo.GetCultures( CultureTypes.SpecificCultures )
                     where !string.IsNullOrEmpty( c.TextInfo.CultureName )
                     orderby c.EnglishName
                     select new {
                       Name = c.EnglishName + " - " + c.TextInfo.CultureName,
                       Id = c.TextInfo.CultureName
                     };
      string cultureJson = "[" + string.Join(",", cultures.Select(c => "{\"name\": \""+c.Name+ "\", \"id\": \"" + c.Id + "\" }" ) ) + "]";
      HttpResponseMessage response = new HttpResponseMessage {
        Content = new StringContent( cultureJson )
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

      return response;
    }


    [HttpPost]
    public bool Save( Currency currencyDTO ) {
      Currency currency = CurrencyService.Instance.Get( currencyDTO.StoreId, currencyDTO.Id );
      currency.Name = currencyDTO.Name;
      currency.IsoCode = currencyDTO.IsoCode;
      currency.PricePropertyAlias = currencyDTO.PricePropertyAlias;
      currency.CultureName = currencyDTO.CultureName;
      currency.AllowedInFollowingCountries = currencyDTO.AllowedInFollowingCountries;
      currency.Symbol = currencyDTO.Symbol;
      currency.SymbolPlacement = currencyDTO.SymbolPlacement;
      currency.Save();

      return true;
    }
  }
}
