using System;
using TeaCommerce.Api.Models;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public interface IIPublishedContentProductInformationExtractor {

    T GetPropertyValue<T>( IPublishedContent model, string propertyAlias, string variantGuid = null, Func<IPublishedContent, bool> func = null, bool useCachedInformation = true );
    long GetStoreId( IPublishedContent model, bool useCachedInformation = true );
    string GetSku( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true );
    string GetName( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true );
    long? GetVatGroupId( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true );
    long? GetLanguageId( IPublishedContent model, bool useCachedInformation = true );
    OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true );
    CustomPropertyCollection GetProperties( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true );
    ProductSnapshot GetSnapshot( IPublishedContent model, string productIdentifier, bool useCachedInformation = true );
    bool HasAccess( long storeId, IPublishedContent model, bool useCachedInformation = true );
  }
}
