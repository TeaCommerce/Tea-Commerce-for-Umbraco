using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Models;
using umbraco;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class ProductInformationExtractor : IProductInformationExtractor {

    protected IXmlNodeProductInformationExtractor XmlNodeProductInformationExtractor;

    public ProductInformationExtractor( IXmlNodeProductInformationExtractor xmlNodeProductInformationExtractor ) {
      XmlNodeProductInformationExtractor = xmlNodeProductInformationExtractor;
    }

    public virtual long GetStoreId( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetStoreId( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual string GetPropertyValue( string productIdentifier, string propertyAlias ) {
      return XmlNodeProductInformationExtractor.GetPropertyValue( library.GetXmlNodeById( productIdentifier ).Current, propertyAlias );
    }

    public virtual string GetSku( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetSku( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual string GetName( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetName( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual long? GetVatGroupId( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetVatGroupId( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual long? GetLanguageId( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetLanguageId( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetOriginalUnitPrices( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual CustomPropertyCollection GetProperties( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetProperties( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual ProductSnapshot GetSnapshot( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetSnapshot( library.GetXmlNodeById( productIdentifier ).Current, productIdentifier );
    }

    public virtual bool HasAccess( long storeId, string productIdentifier ) {
      return XmlNodeProductInformationExtractor.HasAccess( storeId, library.GetXmlNodeById( productIdentifier ).Current );
    }

  }
}
