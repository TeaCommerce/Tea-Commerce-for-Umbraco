using System.Collections.Generic;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public interface IVariantService {

    VariantPublishedContent GetVariant( long storeId, IPublishedContent content, string variantId, bool onlyValid = true );
    VariantPublishedContent GetVariant( long storeId, IContent content, string variantId, bool onlyValid = true );
    IEnumerable<VariantPublishedContent> GetVariants( long storeId, IPublishedContent content, bool onlyValid = true );
    IEnumerable<VariantPublishedContent> GetVariants( long storeId, IContent content, bool onlyValid );
    IEnumerable<VariantAttributeGroup> GetVariantGroups( IEnumerable<VariantPublishedContent> variants );
  }
}