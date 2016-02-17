using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Models;
using umbraco;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class ProductInformationExtractor : IProductInformationExtractor {

    public class ProductIdentifier {
      public string NodeId { get; set; }
      public string VariantGuid { get; set; }

      public ProductIdentifier( string productIdentifier ) {
        if ( productIdentifier.Contains( "_" ) ) {
          NodeId = productIdentifier.Split( '_' )[0];
          VariantGuid = productIdentifier.Split( '_' )[1];
        } else {
          NodeId = productIdentifier;
        }
      }
    }

    protected IXmlNodeProductInformationExtractor XmlNodeProductInformationExtractor;

    public ProductInformationExtractor( IXmlNodeProductInformationExtractor xmlNodeProductInformationExtractor ) {
      XmlNodeProductInformationExtractor = xmlNodeProductInformationExtractor;
    }

    public virtual long GetStoreId( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetStoreId( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual string GetPropertyValue( string productIdentifier, string propertyAlias ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return XmlNodeProductInformationExtractor.GetPropertyValue( library.GetXmlNodeById( productIdentifierObj.NodeId ).Current, productIdentifierObj.VariantGuid, propertyAlias );
    }

    public virtual string GetSku( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return XmlNodeProductInformationExtractor.GetSku( library.GetXmlNodeById( productIdentifierObj.NodeId ).Current, productIdentifierObj.VariantGuid );
    }

    public virtual string GetName( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return XmlNodeProductInformationExtractor.GetName( library.GetXmlNodeById( productIdentifierObj.NodeId ).Current, productIdentifierObj.VariantGuid );
    }

    public virtual long? GetVatGroupId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return XmlNodeProductInformationExtractor.GetVatGroupId( library.GetXmlNodeById( productIdentifierObj.NodeId ).Current, productIdentifierObj.VariantGuid );
    }

    public virtual long? GetLanguageId( string productIdentifier ) {
      return XmlNodeProductInformationExtractor.GetLanguageId( library.GetXmlNodeById( productIdentifier ).Current );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return XmlNodeProductInformationExtractor.GetOriginalUnitPrices( library.GetXmlNodeById( productIdentifierObj.NodeId ).Current, productIdentifierObj.VariantGuid );
    }

    public virtual CustomPropertyCollection GetProperties( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return XmlNodeProductInformationExtractor.GetProperties( library.GetXmlNodeById( productIdentifierObj.NodeId ).Current, productIdentifierObj.VariantGuid );
    }

    public virtual ProductSnapshot GetSnapshot( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return XmlNodeProductInformationExtractor.GetSnapshot( library.GetXmlNodeById( productIdentifierObj.NodeId ).Current, productIdentifier, productIdentifierObj.VariantGuid );
    }

    public virtual bool HasAccess( long storeId, string productIdentifier ) {
      return XmlNodeProductInformationExtractor.HasAccess( storeId, library.GetXmlNodeById( productIdentifier ).Current );
    }

  }
}
