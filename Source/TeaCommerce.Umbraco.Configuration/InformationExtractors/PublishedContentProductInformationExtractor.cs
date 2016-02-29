using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Autofac;
using System;
using System.Linq;
using System.Xml.Linq;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Services;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Dynamics;
using Umbraco.Core.Models;
using Umbraco.Web;
using Constants = TeaCommerce.Api.Constants;
using TeaCommerce.Umbraco.Configuration.Variant;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class PublishedContentProductInformationExtractor : IPublishedContentProductInformationExtractor {

    internal UmbracoHelper UmbracoHelper;

    protected readonly IStoreService StoreService;
    protected readonly ICurrencyService CurrencyService;
    protected readonly IVatGroupService VatGroupService;

    public static IPublishedContentProductInformationExtractor Instance { get { return DependencyContainer.Instance.Resolve<IPublishedContentProductInformationExtractor>(); } }

    public PublishedContentProductInformationExtractor( IStoreService storeService, ICurrencyService currencyService, IVatGroupService vatGroupService ) {
      StoreService = storeService;
      CurrencyService = currencyService;
      VatGroupService = vatGroupService;
      UmbracoHelper = new UmbracoHelper( UmbracoContext.Current );
    }

    public virtual T GetPropertyValue<T>( IPublishedContent model, string propertyAlias, string variantId = null, Func<IPublishedContent, bool> func = null, bool recursive = true ) {
      T rtnValue = default( T );

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        if ( !string.IsNullOrEmpty( variantId ) ) {
          IPublishedContent variant = null;
          long storeId = GetStoreId( model );

          variant = VariantService.Instance.GetVariant( storeId, model, variantId );

          if ( variant != null ) {
            rtnValue = variant.GetPropertyValue<T>( propertyAlias );
          }
        }

        if ( ( string.IsNullOrEmpty( variantId ) || recursive ) && CheckNullOrEmpty( rtnValue ) ) {
          //Check if this node or ancestor has it
          IPublishedContent currentNode = func != null ? model.AncestorsOrSelf().FirstOrDefault( func ) : model;
          if ( currentNode != null ) {
            rtnValue = GetPropertyValueInternal<T>( currentNode, propertyAlias, recursive && func == null );
          }

          //Check if we found the value
          if ( CheckNullOrEmpty( rtnValue ) ) {

            //Check if we can find a master relation
            string masterRelationNodeId = GetPropertyValueInternal<string>( model, Constants.ProductPropertyAliases.MasterRelationPropertyAlias, recursive );
            if ( !string.IsNullOrEmpty( masterRelationNodeId ) ) {
              rtnValue = GetPropertyValue<T>( UmbracoHelper.TypedContent( masterRelationNodeId ), propertyAlias,
                variantId, func );
            }
          }

        }
      }

      return rtnValue;
    }

    protected virtual T GetPropertyValueInternal<T>( IPublishedContent model, string propertyAlias, bool recursive ) {
      T rtnValue = default( T );

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {

        if ( !recursive ) {
          rtnValue = model.GetPropertyValue<T>( propertyAlias );
        } else {
          IPublishedContent tempModel = model;
          T tempProperty = tempModel.GetPropertyValue<T>( propertyAlias );
          if ( !CheckNullOrEmpty( tempProperty ) ) {
            rtnValue = tempProperty;
          }

          while ( CheckNullOrEmpty( rtnValue ) && tempModel != null && tempModel.Id > 0 ) {
            tempModel = tempModel.Parent;
            if ( tempModel != null ) {
              tempProperty = tempModel.GetPropertyValue<T>( propertyAlias );
              if ( !CheckNullOrEmpty( tempProperty ) ) {
                rtnValue = tempProperty;
              }
            }
          }
        }
      }

      return rtnValue;
    }

    public virtual long GetStoreId( IPublishedContent model ) {
      long? storeId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.StorePropertyAlias );
      if ( storeId == null ) {
        throw new ArgumentException( "The model doesn't have a store id associated with it - remember to add the Tea Commerce store picker to your Umbraco content tree" );
      }

      return storeId.Value;
    }

    public virtual string GetSku( IPublishedContent model, string variantId = null ) {
      string sku = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.SkuPropertyAlias, variantId, recursive: string.IsNullOrEmpty( variantId ) );

      //If no sku is found - default to umbraco node id
      if ( string.IsNullOrEmpty( sku ) ) {
        if ( !string.IsNullOrEmpty( variantId ) ) {
          sku = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.SkuPropertyAlias );
        }
        if ( string.IsNullOrEmpty( sku ) ) {
          sku = model.Id.ToString( CultureInfo.InvariantCulture );
        }
        if ( !string.IsNullOrEmpty( variantId ) ) {
          sku += "_" + variantId;
        }
      }

      return sku;
    }

    public virtual string GetName( IPublishedContent model, string variantId = null ) {
      string name = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.NamePropertyAlias, variantId, recursive: string.IsNullOrEmpty( variantId ) );

      //If no name is found - default to the umbraco node name
      if ( string.IsNullOrEmpty( name ) ) {
        if ( !string.IsNullOrEmpty( variantId ) ) {
          name = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.NamePropertyAlias );
        }
        if ( string.IsNullOrEmpty( name ) ) {
          name = model.Name;
        }
        if ( !string.IsNullOrEmpty( variantId ) ) {
          long storeId = GetStoreId( model );
          VariantPublishedContent variant = VariantService.Instance.GetVariant( storeId, model, variantId, false );
          if ( variant != null ) {
            name += " - " + variant.Name;
          }
        }
      }

      return name;
    }

    public virtual long? GetVatGroupId( IPublishedContent model, string variantId = null ) {
      long storeId = GetStoreId( model );
      long? vatGroupId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.VatGroupPropertyAlias, variantId );

      //In umbraco a product can have a relation to a delete marked vat group
      if ( vatGroupId != null ) {
        VatGroup vatGroup = VatGroupService.Get( storeId, vatGroupId.Value );
        if ( vatGroup == null || vatGroup.IsDeleted ) {
          vatGroupId = null;
        }
      }

      return vatGroupId;
    }

    public virtual long? GetLanguageId( IPublishedContent model ) {
      return LanguageService.Instance.GetLanguageIdByNodePath( model.Path );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent model, string variantId = null ) {
      OriginalUnitPriceCollection prices = new OriginalUnitPriceCollection();

      foreach ( Currency currency in CurrencyService.GetAll( GetStoreId( model ) ) ) {
        prices.Add( new OriginalUnitPrice( GetPropertyValue<string>( model, currency.PricePropertyAlias, variantId ).ParseToDecimal() ?? 0M, currency.Id ) );
      }

      return prices;
    }

    public virtual CustomPropertyCollection GetProperties( IPublishedContent model, string variantId = null ) {
      CustomPropertyCollection properties = new CustomPropertyCollection();

      foreach ( string productPropertyAlias in StoreService.Get( GetStoreId( model ) ).ProductSettings.ProductPropertyAliases ) {
        properties.Add( new CustomProperty( productPropertyAlias, GetPropertyValue<string>( model, productPropertyAlias, variantId ) ) { IsReadOnly = true } );
      }

      return properties;
    }

    public virtual ProductSnapshot GetSnapshot( IPublishedContent model, string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      //We use Clone() because each method should have it's own instance of the navigator - so if they traverse it doesn't affect other methods
      ProductSnapshot snapshot = new ProductSnapshot( GetStoreId( model ), productIdentifier ) {
        Sku = GetSku( model, productIdentifierObj.VariantId ),
        Name = GetName( model, productIdentifierObj.VariantId ),
        VatGroupId = GetVatGroupId( model, productIdentifierObj.VariantId ),
        LanguageId = GetLanguageId( model ),
        OriginalUnitPrices = GetOriginalUnitPrices( model, productIdentifierObj.VariantId ),
        Properties = GetProperties( model, productIdentifierObj.VariantId )
      };

      return snapshot;
    }

    public virtual bool HasAccess( long storeId, IPublishedContent model ) {
      return storeId == GetStoreId( model ) && library.HasAccess( model.Id, model.Path );
    }

    private static bool CheckNullOrEmpty<T>( T value ) {
      if ( typeof( T ) == typeof( string ) )
        return string.IsNullOrEmpty( value as string );

      return value == null || value.Equals( default( T ) );
    }
  }
}
