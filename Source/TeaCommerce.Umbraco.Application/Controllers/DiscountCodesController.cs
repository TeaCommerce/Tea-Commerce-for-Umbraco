using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Marketing.Serialization;
using TeaCommerce.Api.Marketing.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace TeaCommerce.Umbraco.Application.Controllers {

  [PluginController( Constants.Applications.TeaCommerce )]
  public class DiscountCodesController : UmbracoAuthorizedApiController {

    [HttpGet]
    public HttpResponseMessage GetCount( long storeId, long ruleId ) {
      List<DiscountCode> discountCodes = DiscountCodeService.Instance.GetAll( storeId, ruleId ).ToList();

      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( "{ \"total\": " + discountCodes.Count + ", \"unused\": " + discountCodes.Count( dc => dc.MaxUses == null || dc.TimesUsed < dc.MaxUses.Value ) + " }", Encoding.UTF8, "application/json" );
      return response;
    }

    [HttpGet]
    public HttpResponseMessage GetDownload( long storeId, long campaignId, long ruleId ) {
      Campaign campaign = CampaignService.Instance.Get( storeId, campaignId );
      if ( campaign == null ) return null;

      List<DiscountCode> unusedDiscountCodes = DiscountCodeService.Instance.GetAll( storeId, ruleId ).Where( dc => dc.MaxUses == null || dc.TimesUsed < dc.MaxUses.Value ).ToList();

      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new PushStreamContent( ( stream, content, context ) => {
        using ( StreamWriter streamWriter = new StreamWriter( stream ) ) {
          streamWriter.Write( string.Join( Environment.NewLine, unusedDiscountCodes.Select( dc => dc.Code ) ) );
          streamWriter.Flush();
        }
      }, "application/octet-stream" );
      response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue( "attachment" ) {
        FileName = "discount codes - " + campaign.Name.ToLowerInvariant() + ".txt"
      };
      return response;
    }

    public class AddPostData {
      public long StoreId { get; set; }
      public long RuleId { get; set; }
      public int? MaxUses { get; set; }
      public string Codes { get; set; }
    }

    public class DiscountCodeLists {
      public List<DiscountCode> DiscountCodes { get; set; }
      public List<DiscountCode> DiscountCodesAlreadyExists { get; set; }
    }

    [HttpPost]
    public HttpResponseMessage Add( AddPostData postData ) {
      DiscountCodeLists discountCodeLists = new DiscountCodeLists();
      List<DiscountCode> discountCodes = new List<DiscountCode>();
      List<DiscountCode> discountCodesAlreadyExists = new List<DiscountCode>();

      foreach ( string code in postData.Codes.Split( new[] { "\n" }, StringSplitOptions.None ) ) {

        DiscountCode doesDiscountCodeAlreadyExist = DiscountCodeService.Instance.Get( postData.StoreId, code );
        if ( doesDiscountCodeAlreadyExist == null ) {
          DiscountCode discountCode = new DiscountCode( postData.RuleId, code ) {
            MaxUses = postData.MaxUses
          };
          discountCode.Save();
          discountCodes.Add( discountCode );
        } else {
          discountCodesAlreadyExists.Add( doesDiscountCodeAlreadyExist );
        }
      }

      discountCodeLists.DiscountCodes = discountCodes;
      discountCodeLists.DiscountCodesAlreadyExists = discountCodesAlreadyExists;

      string discountCodeListsJson = new JavaScriptSerializer().Serialize( discountCodeLists );

      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( discountCodeListsJson, Encoding.UTF8, "application/json" );
      return response;
    }

    public class GeneratePostData {
      public long StoreId { get; set; }
      public long RuleId { get; set; }
      public int NumberToGenerate { get; set; }
      public int? MaxUses { get; set; }
      public int Length { get; set; }
      public string Prefix { get; set; }
      public string Postfix { get; set; }
    }

    [HttpPost]
    public HttpResponseMessage Generate( GeneratePostData postData ) {
      HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.OK );
      response.Content = new StringContent( DiscountCodeService.Instance.Generate( postData.StoreId, postData.RuleId, postData.NumberToGenerate, postData.MaxUses, postData.Length, postData.Prefix, postData.Postfix ).ToJson(), Encoding.UTF8, "application/json" );
      return response;
    }

  }
}