using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public class VariantService {
    private static VariantService _instance;
    public static VariantService Instance {
      get { return _instance ?? ( _instance = new VariantService() ); }
    }

    private VariantService() { }

    public VariantPublishedContent GetVariants( int nodeId, string variantId ) {
      List<VariantPublishedContent> variants = GetVariants( nodeId );

      return variants.FirstOrDefault( v => v.VariantId == variantId );
    }

    public List<VariantPublishedContent> GetVariants( int nodeId ) {
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );

      IPublishedContent content = umbracoHelper.TypedContent( nodeId );

      return GetVariants( content );
    }

    public List<VariantPublishedContent> GetVariants( IPublishedContent content ) {
      List<VariantPublishedContent> variants = new List<VariantPublishedContent>();

      if ( content != null ) {
        string variantsJson = content.GetPropertyValue<string>( "variants" ); //TODO: Er "variants" altid det samme?

        List<Variant.Product.Variant> productVariants = JObject.Parse( variantsJson ).SelectToken( "variants" ).ToObject<List<Variant.Product.Variant>>();

        foreach ( Variant.Product.Variant variant in productVariants ) {
          variant.DocumentTypeAlias = "Variant"; //TODO: Get real variant documenttype

          PublishedContentType publishedContentType = PublishedContentType.Get( PublishedItemType.Content, variant.DocumentTypeAlias );

          variants.Add( new VariantPublishedContent( variant, publishedContentType, content ) );
        }
      }

      return variants;
    }

    public Dictionary<int, string> GetVariantGroups( List<VariantPublishedContent> variants ) {
      Dictionary<int, string> variantGroups = new Dictionary<int, string>();

      foreach ( VariantPublishedContent variant in variants ) {
        foreach ( Combination combination in variant.Combinations ) {
          if ( !variantGroups.ContainsKey( combination.GroupId ) ) {
            variantGroups.Add( combination.GroupId, combination.GroupName );
          }
        }
      }

      return variantGroups;
    }
  }
}
