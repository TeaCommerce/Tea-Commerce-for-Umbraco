using System.Xml.XPath;
using TeaCommerce.Api.Models;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public interface IXmlNodeProductInformationExtractor {

    string GetPropertyValue( XPathNavigator model, string variantGuid, string propertyAlias, string selector = null, bool useCachedInformation = true );
    XPathNavigator GetXmlPropertyValue( XPathNavigator model, string variantGuid, string propertyAlias, string selector = null, bool useCachedInformation = true );
    long GetStoreId( XPathNavigator model, bool useCachedInformation = true );
    string GetSku( XPathNavigator model, string variantGuid, bool useCachedInformation = true );
    string GetName( XPathNavigator model, string variantGuid, bool useCachedInformation = true );
    long? GetVatGroupId( XPathNavigator model, string variantGuid, bool useCachedInformation = true );
    long? GetLanguageId( XPathNavigator model, bool useCachedInformation = true );
    OriginalUnitPriceCollection GetOriginalUnitPrices( XPathNavigator model, string variantGuid, bool useCachedInformation = true );
    CustomPropertyCollection GetProperties( XPathNavigator model, string variantGuid, bool useCachedInformation = true );
    ProductSnapshot GetSnapshot( XPathNavigator model, string variantGuid, string productIdentifier, bool useCachedInformation = true );
    bool HasAccess( long storeId, XPathNavigator model, bool useCachedInformation = true );
  }
}
