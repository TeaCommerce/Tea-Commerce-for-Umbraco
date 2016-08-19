using System;
using System.Collections.Generic;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public interface IVariantService<T> {

    VariantPublishedContent<T> GetVariant( long storeId, T content, string variantId, bool onlyValid = true );
    IEnumerable<VariantPublishedContent<T>> GetVariants( long storeId, T content, bool onlyValid = true );
    IEnumerable<VariantGroup> GetVariantGroups( IEnumerable<VariantPublishedContent<T>> variants );
    string GetVariantDataFromContent( long storeId, T productContents, bool onlyValid );
    string GetVariantJson( long storeId, IEnumerable<T> productContents, bool onlyValid );
    List<VariantPublishedContent<T>> ParseVariantJson( string json, T parentContent );
    int GetId( T content );
    string GetVariantProductIdentifier( T content, VariantPublishedContent<T> variant );
  }
}