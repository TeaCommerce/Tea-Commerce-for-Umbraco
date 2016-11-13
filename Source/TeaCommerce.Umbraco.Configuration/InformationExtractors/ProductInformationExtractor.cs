using Autofac;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Configuration.Services;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {

  public class ProductInformationExtractorT : IProductInformationExtractor<IPublishedContent, VariantPublishedContent<IPublishedContent>> {

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

    public virtual string GetPropertyValue( IPublishedContent product, VariantPublishedContent<IPublishedContent> variant, string propertyAlias ) {
      return IPublishedContentProductInformationExtractor.GetPropertyValue<string>( product, propertyAlias, variant );
    }

    public virtual string GetSku( IPublishedContent product, VariantPublishedContent<IPublishedContent> variant ) {
      return IPublishedContentProductInformationExtractor.GetSku( product, variant );
    }

    public virtual string GetName( IPublishedContent product, VariantPublishedContent<IPublishedContent> variant ) {
      return IPublishedContentProductInformationExtractor.GetName( product, variant );
    }

    public virtual long? GetVatGroupId( IPublishedContent product, VariantPublishedContent<IPublishedContent> variant ) {
      return IPublishedContentProductInformationExtractor.GetVatGroupId( product, variant );
    }

    public virtual long? GetLanguageId( IPublishedContent product ) {
      return IPublishedContentProductInformationExtractor.GetLanguageId( product );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent product, VariantPublishedContent<IPublishedContent> variant ) {
      return IPublishedContentProductInformationExtractor.GetOriginalUnitPrices( product, variant );
    }

    public virtual CustomPropertyCollection GetProperties( IPublishedContent product, VariantPublishedContent<IPublishedContent> variant ) {
      return IPublishedContentProductInformationExtractor.GetProperties( product, variant );
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
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );
      long storeId = IPublishedContentProductInformationExtractor.GetStoreId( content );
      VariantPublishedContent<IPublishedContent> variant = PublishedContentVariantService.Instance.GetVariant( storeId, content, productIdentifierObj.VariantId );
      return IPublishedContentProductInformationExtractor.GetSku( content, variant );
    }

    public virtual long? GetVatGroupId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );
      long storeId = IPublishedContentProductInformationExtractor.GetStoreId( content );
      VariantPublishedContent<IPublishedContent> variant = PublishedContentVariantService.Instance.GetVariant( storeId, content, productIdentifierObj.VariantId );
      return IPublishedContentProductInformationExtractor.GetVatGroupId( content, variant );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );
      long storeId = IPublishedContentProductInformationExtractor.GetStoreId( content );
      VariantPublishedContent<IPublishedContent> variant = PublishedContentVariantService.Instance.GetVariant( storeId, content, productIdentifierObj.VariantId );
      return IPublishedContentProductInformationExtractor.GetOriginalUnitPrices( content, variant );
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
