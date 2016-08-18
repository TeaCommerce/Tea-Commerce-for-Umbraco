using Autofac;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {

  public class ProductInformationExtractorT : IProductInformationExtractor<IPublishedContent, string> {

    protected IPublishedContentProductInformationExtractor IPublishedContentProductInformationExtractor;
    protected UmbracoHelper UmbracoHelper { get; private set; }

    public static IProductInformationExtractor Instance { get { return DependencyContainer.Instance.Resolve<IProductInformationExtractor>(); } }

    public ProductInformationExtractorT( IPublishedContentProductInformationExtractor iPublishedContentProductInformationExtractor ) {
      IPublishedContentProductInformationExtractor = iPublishedContentProductInformationExtractor;
      UmbracoHelper = new UmbracoHelper( UmbracoContext.Current );
    }

    public virtual long GetStoreId( IPublishedContent product) {
      return IPublishedContentProductInformationExtractor.GetStoreId( product );
    }

    public virtual string GetPropertyValue( IPublishedContent product, string variantId, string propertyAlias ) {
      return IPublishedContentProductInformationExtractor.GetPropertyValue<string>( product, variantId, propertyAlias );
    }

    public virtual string GetSku( IPublishedContent product, string variantId ) {
      return IPublishedContentProductInformationExtractor.GetSku( product, variantId );
    }

    public virtual string GetName( IPublishedContent product, string variantId ) {
      return IPublishedContentProductInformationExtractor.GetName( product, variantId );
    }

    public virtual long? GetVatGroupId( IPublishedContent product, string variantId ) {
      return IPublishedContentProductInformationExtractor.GetVatGroupId( product, variantId );
    }

    public virtual long? GetLanguageId( IPublishedContent product ) {
      return IPublishedContentProductInformationExtractor.GetLanguageId( product );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent product, string variantId ) {
      return IPublishedContentProductInformationExtractor.GetOriginalUnitPrices( product, variantId );
    }

    public virtual CustomPropertyCollection GetProperties( IPublishedContent product, string variantId ) {
      return IPublishedContentProductInformationExtractor.GetProperties( product, variantId );
    }

  }



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

    public virtual string GetSku( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetSku( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
    }

    public virtual long? GetVatGroupId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetVatGroupId( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return IPublishedContentProductInformationExtractor.GetOriginalUnitPrices( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ), productIdentifierObj.VariantId );
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
