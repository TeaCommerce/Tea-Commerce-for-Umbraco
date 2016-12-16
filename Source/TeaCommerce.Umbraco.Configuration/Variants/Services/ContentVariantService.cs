using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Variants.Services {

  public class ContentVariantService : AVariantPublishedContentService<IContent> {
    public override int GetId( IContent content ) {
      return content.Id;
    }

    public override string GetVariantDataFromContent( long storeId, IContent content, bool onlyValid ) {
      string variantsJson = "";

      if ( content != null ) {
        Store store = StoreService.Instance.Get( storeId );
        if ( content.HasProperty( store.ProductSettings.ProductVariantPropertyAlias ) ) {
          variantsJson = content.GetValue<string>( store.ProductSettings.ProductVariantPropertyAlias );
        } else {
          LogHelper.Debug<PublishedContentVariantService>( "There was no variants in the property \"" + store.ProductSettings.ProductVariantPropertyAlias + "\". Check the " + content.ContentType.Alias + " doc type and the product variant alias setting on the \"" + store.Name + "\" store." );
        }
      }

      return variantsJson;
    }

    public override string GetVariantProductIdentifier( IContent content, VariantPublishedContent variant ) {
      return content.Id + "_" + variant.VariantIdentifier;
    }
  }
}
