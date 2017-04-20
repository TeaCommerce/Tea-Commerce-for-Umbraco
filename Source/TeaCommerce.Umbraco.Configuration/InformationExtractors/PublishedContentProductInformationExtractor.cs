using System;
using System.Globalization;
using System.Linq;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Services;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using TeaCommerce.Umbraco.Configuration.Variants.Services;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;
using Constants = TeaCommerce.Api.Constants;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class PublishedContentProductInformationExtractor : IProductInformationExtractor, IProductInformationExtractor<IPublishedContent, VariantPublishedContent>, IPublishedContentProductInformationExtractor, IProductInformationExtractor<IPublishedContent> {

    private UmbracoHelper _umbracoHelper;
    protected UmbracoHelper UmbracoHelper {
      get {
        return _umbracoHelper = _umbracoHelper ?? new UmbracoHelper( UmbracoContext.Current );
      }
    }

    protected readonly IStoreService StoreService;
    protected readonly ICurrencyService CurrencyService;
    protected readonly IVatGroupService VatGroupService;
    protected readonly IVariantService<IPublishedContent, VariantPublishedContent> VariantService;

    public PublishedContentProductInformationExtractor( IStoreService storeService, ICurrencyService currencyService, IVatGroupService vatGroupService, IVariantService<IPublishedContent, VariantPublishedContent> variantService ) {
      StoreService = storeService;
      CurrencyService = currencyService;
      VatGroupService = vatGroupService;
      VariantService = variantService;
    }

    #region IProductInformationExtractor

    public virtual long GetStoreId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      return GetStoreId( UmbracoHelper.TypedContent( productIdentifierObj.NodeId ) );
    }

    public virtual string GetSku( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );
      long storeId = GetStoreId( content );
      VariantPublishedContent variant = VariantService.GetVariant( storeId, content, productIdentifierObj.VariantId );
      return GetSku( content, variant );
    }

    public virtual long? GetVatGroupId( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );
      long storeId = GetStoreId( content );
      VariantPublishedContent variant = VariantService.GetVariant( storeId, content, productIdentifierObj.VariantId );
      return GetVatGroupId( content, variant );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );
      long storeId = GetStoreId( content );
      VariantPublishedContent variant = VariantService.GetVariant( storeId, content, productIdentifierObj.VariantId );
      return GetOriginalUnitPrices( content, variant );
    }

    public virtual ProductSnapshot GetSnapshot( string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );

      long storeId = GetStoreId( content );
      VariantPublishedContent variant = VariantService.GetVariant( storeId, content, productIdentifierObj.VariantId );

      ProductSnapshot snapshot = new ProductSnapshot( storeId, productIdentifier ) {
        Sku = GetSku( content, variant ),
        Name = GetName( content, variant ),
        VatGroupId = GetVatGroupId( content, variant ),
        LanguageId = GetLanguageId( content ),
        OriginalUnitPrices = GetOriginalUnitPrices( content, variant ),
        Properties = GetProperties( content, variant )
      };

      return snapshot;
    }

    public virtual bool HasAccess( long storeId, string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );

      return storeId == GetStoreId( content ) && library.HasAccess( content.Id, content.Path );
    }

    #endregion

    #region IProductInformationExtractor<IPublishedContent, VariantPublishedContent>

    public virtual long GetStoreId( IPublishedContent product, VariantPublishedContent variant ) {
      long? storeId = GetPropertyValue<long?>( product, Constants.ProductPropertyAliases.StorePropertyAlias );
      if ( storeId == null ) {
        throw new ArgumentException( "The product doesn't have a store id associated with it - remember to add the Tea Commerce store picker to your Umbraco content tree" );
      }

      return storeId.Value;
    }

    public virtual string GetSku( IPublishedContent product, VariantPublishedContent variant = null ) {
      string sku = GetPropertyValue<string>( product, Constants.ProductPropertyAliases.SkuPropertyAlias, variant, recursive: variant == null );

      //If no sku is found - default to umbraco node id
      if ( string.IsNullOrEmpty( sku ) ) {
        sku = product.Id.ToString( CultureInfo.InvariantCulture ) + ( variant != null ? "_" + variant.VariantIdentifier : "" );
      }

      return sku;
    }

    public virtual string GetName( IPublishedContent product, VariantPublishedContent variant = null ) {
      string name = GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias, variant, recursive: variant == null );

      //If no name is found - default to the umbraco node name
      if ( string.IsNullOrEmpty( name ) ) {
        if ( variant != null ) {
          name = GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias );
        }
        if ( variant == null ) {
          name = product.Name;
        }
        if ( variant != null ) {
          name += " - " + variant.Name;
        }
      }

      return name;
    }

    public virtual long? GetVatGroupId( IPublishedContent product, VariantPublishedContent variant = null ) {
      long storeId = GetStoreId( product );
      long? vatGroupId = GetPropertyValue<long?>( product, Constants.ProductPropertyAliases.VatGroupPropertyAlias, variant );

      //In umbraco a product can have a relation to a delete marked vat group
      if ( vatGroupId != null ) {
        VatGroup vatGroup = VatGroupService.Get( storeId, vatGroupId.Value );
        if ( vatGroup == null || vatGroup.IsDeleted ) {
          vatGroupId = null;
        }
      }

      return vatGroupId;
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent product, VariantPublishedContent variant = null ) {
      OriginalUnitPriceCollection prices = new OriginalUnitPriceCollection();

      foreach ( Currency currency in CurrencyService.GetAll( GetStoreId( product ) ) ) {
        prices.Add( new OriginalUnitPrice( GetPropertyValue<string>( product, currency.PricePropertyAlias, variant ).ParseToDecimal() ?? 0M, currency.Id ) );
      }

      return prices;
    }

    public virtual long? GetLanguageId( IPublishedContent product, VariantPublishedContent variant ) {
      return LanguageService.Instance.GetLanguageIdByNodePath( product.Path );
    }

    public virtual CustomPropertyCollection GetProperties( IPublishedContent product, VariantPublishedContent variant = null ) {
      CustomPropertyCollection properties = new CustomPropertyCollection();

      foreach ( string productPropertyAlias in StoreService.Get( GetStoreId( product ) ).ProductSettings.ProductPropertyAliases ) {
        properties.Add( new CustomProperty( productPropertyAlias, GetPropertyValue<string>( product, productPropertyAlias, variant ) ) { IsReadOnly = true } );
      }

      return properties;
    }

    public virtual string GetPropertyValue( IPublishedContent product, string propertyAlias, VariantPublishedContent variant = null ) {
      return GetPropertyValue<string>( product, propertyAlias, variant );
    }

    #endregion

    #region IProductInformationExtractor<IPublishedContent>

    public long GetStoreId( IPublishedContent product ) {
      return GetStoreId( product, null );
    }

    public string GetSku( IPublishedContent product ) {
      return GetSku( product, null );
    }

    public string GetName( IPublishedContent product ) {
      return GetName( product, null );
    }

    public long? GetVatGroupId( IPublishedContent product ) {
      return GetVatGroupId( product, null );
    }

    public OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent product ) {
      return GetOriginalUnitPrices( product, null );
    }

    public long? GetLanguageId( IPublishedContent product ) {
      return GetLanguageId( product, null );
    }

    public CustomPropertyCollection GetProperties( IPublishedContent product ) {
      return GetProperties( product, null );
    }

    public string GetPropertyValue( IPublishedContent product, string propertyAlias ) {
      return GetPropertyValue( product, propertyAlias, null );
    }

    #endregion

    public virtual T GetPropertyValue<T>( IPublishedContent product, string propertyAlias, VariantPublishedContent variant = null, Func<IPublishedContent, bool> func = null, bool recursive = true ) {
      T rtnValue = default( T );

      if ( product != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        if ( variant != null ) {
          rtnValue = variant.GetPropertyValue<T>( propertyAlias );
        }

        if ( ( variant == null || recursive ) && CheckNullOrEmpty( rtnValue ) ) {
          //Check if this node or ancestor has it
          IPublishedContent currentNode = func != null ? product.AncestorsOrSelf().FirstOrDefault( func ) : product;
          if ( currentNode != null ) {
            rtnValue = GetPropertyValueInternal<T>( currentNode, propertyAlias, recursive && func == null );
          }

          //Check if we found the value
          if ( CheckNullOrEmpty( rtnValue ) ) {

            //Check if we can find a master relation
            string masterRelationNodeId = GetPropertyValueInternal<string>( product, Constants.ProductPropertyAliases.MasterRelationPropertyAlias, recursive );
            if ( !string.IsNullOrEmpty( masterRelationNodeId ) && UmbracoHelper != null ) {
              rtnValue = GetPropertyValue<T>( UmbracoHelper.TypedContent( masterRelationNodeId ), propertyAlias,
                variant, func );
            }
          }

        }
      }

      return rtnValue;
    }

    protected virtual T GetPropertyValueInternal<T>( IPublishedContent product, string propertyAlias, bool recursive ) {
      T rtnValue = default( T );

      if ( product != null && !string.IsNullOrEmpty( propertyAlias ) ) {

        if ( !recursive ) {
          rtnValue = product.GetPropertyValue<T>( propertyAlias );
        } else {
          IPublishedContent tempproduct = product;
          T tempProperty = tempproduct.GetPropertyValue<T>( propertyAlias );
          if ( !CheckNullOrEmpty( tempProperty ) ) {
            rtnValue = tempProperty;
          }

          while ( CheckNullOrEmpty( rtnValue ) && tempproduct != null && tempproduct.Id > 0 ) {
            tempproduct = tempproduct.Parent;
            if ( tempproduct != null ) {
              tempProperty = tempproduct.GetPropertyValue<T>( propertyAlias );
              if ( !CheckNullOrEmpty( tempProperty ) ) {
                rtnValue = tempProperty;
              }
            }
          }
        }
      }

      return rtnValue;
    }

    protected bool CheckNullOrEmpty<T>( T value ) {
      if ( typeof( T ) == typeof( string ) )
        return string.IsNullOrEmpty( value as string );

      return value == null || value.Equals( default( T ) );
    }
  }
}
