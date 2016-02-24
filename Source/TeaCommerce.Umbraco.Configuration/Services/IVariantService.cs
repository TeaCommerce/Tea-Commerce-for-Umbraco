using System.Collections.Generic;
using TeaCommerce.Umbraco.Configuration.Variant;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public interface IVariantService {

    VariantPublishedContent GetVariant( long storeId, IPublishedContent content, string variantId, bool onlyValid = true );
    VariantPublishedContent GetVariant( long storeId, IContent content, string variantId, bool onlyValid = true );
    List<VariantPublishedContent> GetVariants( long storeId, IPublishedContent content, bool onlyValid = true );
    List<VariantPublishedContent> GetVariants( long storeId, IContent content, bool onlyValid );
    Dictionary<int, string> GetVariantGroups( List<VariantPublishedContent> variants );

  }
}