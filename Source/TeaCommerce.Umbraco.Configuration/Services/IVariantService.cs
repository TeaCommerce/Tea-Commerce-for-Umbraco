using System.Collections.Generic;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public interface IVariantService<T> {

    VariantPublishedContent GetVariant( long storeId, T content, string variantId, bool onlyValid = true );
    IEnumerable<VariantPublishedContent> GetVariants( long storeId, T content, bool onlyValid = true );
    IEnumerable<VariantGroup> GetVariantGroups( IEnumerable<VariantPublishedContent> variants );
    string GetVariantDataFromContent( long storeId, T productContents, bool onlyValid );
    string GetVariantJson( long storeId, IEnumerable<T> productContents, bool onlyValid );
    List<VariantPublishedContent> ParseVariantJson( string json, IPublishedContent parentContent );
    int GetId( T content );
  }
}