using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using umbraco;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class ProductInformationExtractor : IProductInformationExtractor {

    protected IPublishedContentProductInformationExtractor IPublishedContentProductInformationExtractor;
    protected UmbracoHelper UmbracoHelper { get; private set; }

    public ProductInformationExtractor( IPublishedContentProductInformationExtractor iPublishedContentProductInformationExtractor ) {
      IPublishedContentProductInformationExtractor = iPublishedContentProductInformationExtractor;
      UmbracoHelper = new UmbracoHelper( UmbracoContext.Current );
    }

    public virtual long GetStoreId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetStoreId( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ) );
    }

    public virtual string GetPropertyValue( string productIdentifier, string propertyAlias ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetPropertyValue<string>( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId, propertyAlias );
    }

    public virtual string GetSku( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetSku( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
    }

    public virtual string GetName( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetName( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
    }

    public virtual long? GetVatGroupId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetVatGroupId( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
    }

    public virtual long? GetLanguageId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetLanguageId( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ) );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetOriginalUnitPrices( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
    }

    public virtual CustomPropertyCollection GetProperties( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetProperties( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
    }

    public virtual ProductSnapshot GetSnapshot( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetSnapshot( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifier );
    }

    public virtual bool HasAccess( long storeId, string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.HasAccess( storeId, UmbracoHelper.TypedContent( productIdentifierObj.NodeId ) );
    }

  }
}
