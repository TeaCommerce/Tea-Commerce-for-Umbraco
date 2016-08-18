using System.Collections.Generic;
using System.Linq;
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
using System;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public abstract class AVariantService<T> : IVariantService<T> {

    public string CacheKey = "TeaCommerceVariants";

    public static IVariantService<T> Instance { get { return DependencyContainer.Instance.Resolve<IVariantService<T>>(); } }

    public virtual VariantPublishedContent GetVariant( long storeId, T content, string variantId, bool onlyValid = true ) {
      IEnumerable<VariantPublishedContent> variants = GetVariants( storeId, content, onlyValid );

      return variants.FirstOrDefault( v => v.VariantId == variantId );
    }

    public virtual IEnumerable<VariantPublishedContent> GetVariants( long storeId, T content, bool onlyValid ) {
      IPublishedContent parentContent = content.GetType() == typeof( IPublishedContent ) ? (IPublishedContent)content : null;
      return PublishedContentVariantService.Instance.ParseVariantJson( GetVariantDataFromContent( storeId, content, onlyValid ), parentContent );
    }

    public virtual IEnumerable<VariantGroup> GetVariantGroups( IEnumerable<VariantPublishedContent> variants ) {
      List<VariantGroup> attributeGroups = new List<VariantGroup>();

      foreach ( VariantPublishedContent variant in variants ) {
        foreach ( Combination combination in variant.Combinations ) {
          VariantGroup attributeGroup = attributeGroups.FirstOrDefault( ag => ag.Id == combination.GroupId );

          if ( attributeGroup == null ) {
            attributeGroup = new VariantGroup { Id = combination.GroupId, Name = combination.GroupName };
            attributeGroups.Add( attributeGroup );
          }

          if ( attributeGroup.Attributes.All( a => a.Id != combination.Id ) ) {
            attributeGroup.Attributes.Add( new VariantType { Id = combination.Id, Name = combination.Name } );
          }
        }
      }

      return attributeGroups;
    }

    public virtual string GetVariantJson( long storeId, IEnumerable<T> productContents, bool onlyValid ) {
      Dictionary<int, Dictionary<string, dynamic>> jsonProducts = new Dictionary<int, Dictionary<string, dynamic>>();

      foreach ( T productContent in productContents ) {
        Dictionary<string, dynamic> variants = GetVariants( storeId, productContent, onlyValid ).ToDictionary( v => v.VariantId, v =>
        (dynamic)new {
          combinations = v.Combinations.Select( c => c.Id ),
          productIdentifier = v.ProductIdentifier,
        } );
        jsonProducts.Add( GetId( productContent ), variants );
      }

      return JsonConvert.SerializeObject( jsonProducts );
    }

    public virtual List<VariantPublishedContent> ParseVariantJson( string json, IPublishedContent parentContent ) {
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

    public abstract int GetId( T content );

    public abstract string GetVariantDataFromContent( long storeId, T productContents, bool onlyValid );

    private object GetDefaultValue( Type t ) {
      if ( t.IsValueType && Nullable.GetUnderlyingType( t ) == null ) {
        return Activator.CreateInstance( t );
      } else {
        return null;
      }
    }
  }
}
