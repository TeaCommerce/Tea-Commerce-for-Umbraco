using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public class VariantService : IVariantService {

    public string CacheKey = "TeaCommerceVariants";

    public static IVariantService Instance { get { return DependencyContainer.Instance.Resolve<IVariantService>(); } }

    public VariantPublishedContent GetVariant( long storeId, IPublishedContent content, string variantId, bool onlyValid = true ) {
      List<VariantPublishedContent> variants = GetVariants( storeId, content, onlyValid );

      return variants.FirstOrDefault( v => v.VariantId == variantId );
    }

    public VariantPublishedContent GetVariant( long storeId, IContent content, string variantId, bool onlyValid = true ) {
      List<VariantPublishedContent> variants = GetVariants( storeId, content, onlyValid );

      return variants.FirstOrDefault( v => v.VariantId == variantId );
    }

    public List<VariantPublishedContent> GetVariants( long storeId, IPublishedContent content, bool onlyValid = true ) {
      Dictionary<int, List<VariantPublishedContent>> variantsForProducts = CacheService.Instance.Get<Dictionary<int, List<VariantPublishedContent>>>( CacheKey + "-" + storeId );
      if ( variantsForProducts == null ) {
        variantsForProducts = new Dictionary<int, List<VariantPublishedContent>>();
        CacheService.Instance.Set( CacheKey + "-" + storeId, variantsForProducts );
      }

      List<VariantPublishedContent> variants = new List<VariantPublishedContent>();

      if ( content != null && !variantsForProducts.TryGetValue( content.Id, out variants ) ) {
        Store store = StoreService.Instance.Get( storeId );

        string variantsJson = content.GetPropertyValue<string>( store.ProductSettings.ProductVariantPropertyAlias );

        variants = ParseVariantJson( variantsJson, content );

        if ( onlyValid ) {
          variants = variants.Where( v => !v.Validation.DuplicatesFound && !v.Validation.HolesInVariants ).ToList();
        }
        variantsForProducts.Add( content.Id, variants );
      }

      return variants;
    }

    public List<VariantPublishedContent> GetVariants( long storeId, IContent content, bool onlyValid ) {
      List<VariantPublishedContent> variants = new List<VariantPublishedContent>();
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );

      if ( content != null ) {
        Store store = StoreService.Instance.Get( storeId );

        string variantsJson = content.GetValue<string>( store.ProductSettings.ProductVariantPropertyAlias );
        IPublishedContent parentContent = umbracoHelper.TypedContent( content.Id );
        variants = ParseVariantJson( variantsJson, parentContent );
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

    private List<VariantPublishedContent> ParseVariantJson( string json, IPublishedContent parentContent ) {
      List<VariantPublishedContent> variants = new List<VariantPublishedContent>();
      if ( !string.IsNullOrEmpty( json ) ) {
        List<Variant.Product.Variant> productVariants = JObject.Parse( json ).SelectToken( "variants" ).ToObject<List<Variant.Product.Variant>>();

        foreach ( Variant.Product.Variant variant in productVariants ) {

          PublishedContentType publishedContentType = PublishedContentType.Get( PublishedItemType.Content, variant.DocumentTypeAlias );

          variants.Add( new VariantPublishedContent( variant, publishedContentType, parentContent ) );
        }
      }
      return variants;
    }
  }
}
