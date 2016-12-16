using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Configuration.Variants.Services {
  public abstract class AVariantPublishedContentService<T> : AVariantService<T, VariantPublishedContent> {

    public override IEnumerable<VariantPublishedContent> GetVariants( long storeId, T product, bool onlyValid = true ) {
      return ParseVariantJson( GetVariantDataFromContent( storeId, product, onlyValid ) );
    }

    public abstract string GetVariantDataFromContent( long storeId, T productContents, bool onlyValid );

    public override string GetVariantJson( long storeId, IEnumerable<T> productContents, bool onlyValid ) {
      Dictionary<int, Dictionary<string, dynamic>> jsonProducts = new Dictionary<int, Dictionary<string, dynamic>>();

      foreach ( T productContent in productContents ) {
        Dictionary<string, dynamic> variants = GetVariants( storeId, productContent, onlyValid ).ToDictionary( v => v.VariantIdentifier, v =>
        (dynamic)new {
          combinations = v.Combination.Select( c => c.Id ),
          productIdentifier = GetVariantProductIdentifier( productContent, v ),
        } );
        jsonProducts.Add( GetId( productContent ), variants );
      }

      return JsonConvert.SerializeObject( jsonProducts );
    }

    public virtual List<VariantPublishedContent> ParseVariantJson( string json ) {
      List<VariantPublishedContent> variants = new List<VariantPublishedContent>();
      if ( !string.IsNullOrEmpty( json ) ) {
        List<Variant> productVariants = JObject.Parse( json ).SelectToken( "variants" ).ToObject<List<Variant>>();

        foreach ( Variant variant in productVariants ) {
          PublishedContentType publishedContentType = null;
          if ( !string.IsNullOrEmpty( variant.DocumentTypeAlias ) ) {
            publishedContentType = PublishedContentType.Get( PublishedItemType.Content, variant.DocumentTypeAlias );
          }

          variants.Add( new VariantPublishedContent( variant, publishedContentType ) );
        }
      }
      return variants;
    }

    public abstract int GetId( T content );

    public abstract string GetVariantProductIdentifier( T content, VariantPublishedContent variant );

  }
}
