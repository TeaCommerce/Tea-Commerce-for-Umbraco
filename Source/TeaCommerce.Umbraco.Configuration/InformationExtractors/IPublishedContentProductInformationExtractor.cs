using System;
using TeaCommerce.Api.Models;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public interface IPublishedContentProductInformationExtractor {

    T GetPropertyValue<T>( IPublishedContent model, string propertyAlias, string variantGuid = null, Func<IPublishedContent, bool> func = null, bool recursive = true );
    long GetStoreId( IPublishedContent model );
    string GetSku( IPublishedContent model, string variantGuid = null );
    string GetName( IPublishedContent model, string variantGuid = null );
    long? GetVatGroupId( IPublishedContent model, string variantGuid = null );
    long? GetLanguageId( IPublishedContent model );
    OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent model, string variantGuid = null );
    CustomPropertyCollection GetProperties( IPublishedContent model, string variantGuid = null );
    ProductSnapshot GetSnapshot( IPublishedContent model, string productIdentifier );
    bool HasAccess( long storeId, IPublishedContent model );
  }
}
