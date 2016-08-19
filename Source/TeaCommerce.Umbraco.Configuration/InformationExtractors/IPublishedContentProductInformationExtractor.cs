using System;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Configuration.Variant;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public interface IPublishedContentProductInformationExtractor {

    T GetPropertyValue<T>( IPublishedContent model, string propertyAlias, VariantPublishedContent<IPublishedContent> variant = null, Func<IPublishedContent, bool> func = null, bool recursive = true );
    long GetStoreId( IPublishedContent model );
    string GetSku( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null );
    string GetName( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null );
    long? GetVatGroupId( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null );
    long? GetLanguageId( IPublishedContent model );
    OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null );
    CustomPropertyCollection GetProperties( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null );
    ProductSnapshot GetSnapshot( IPublishedContent model, string productIdentifier );
    bool HasAccess( long storeId, IPublishedContent model );
  }
}
