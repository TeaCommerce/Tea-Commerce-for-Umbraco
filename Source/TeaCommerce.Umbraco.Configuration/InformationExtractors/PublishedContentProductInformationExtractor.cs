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
    protected readonly IVariantService<IPublishedContent> VariantService;

    public static IPublishedContentProductInformationExtractor Instance { get { return DependencyContainer.Instance.Resolve<IPublishedContentProductInformationExtractor>(); } }

    public PublishedContentProductInformationExtractor( IStoreService storeService, ICurrencyService currencyService, IVatGroupService vatGroupService, IVariantService<IPublishedContent> variantService ) {
      StoreService = storeService;
      CurrencyService = currencyService;
      VatGroupService = vatGroupService;
      VariantService = variantService;
      try {
        UmbracoHelper = new UmbracoHelper( UmbracoContext.Current );
      } catch ( Exception ex ) {
        //Will fail if we're out of context
      }
    }

    public virtual T GetPropertyValue<T>( IPublishedContent model, string propertyAlias, VariantPublishedContent<IPublishedContent> variant = null, Func<IPublishedContent, bool> func = null, bool recursive = true ) {
      T rtnValue = default( T );

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        if ( variant != null ) {
          rtnValue = variant.GetPropertyValue<T>( propertyAlias );
        }

        if ( ( variant == null || recursive ) && CheckNullOrEmpty( rtnValue ) ) {
          //Check if this node or ancestor has it
          IPublishedContent currentNode = func != null ? model.AncestorsOrSelf().FirstOrDefault( func ) : model;
          if ( currentNode != null ) {
            rtnValue = GetPropertyValueInternal<T>( currentNode, propertyAlias, recursive && func == null );
          }

          //Check if we found the value
          if ( CheckNullOrEmpty( rtnValue ) ) {

            //Check if we can find a master relation
            string masterRelationNodeId = GetPropertyValueInternal<string>( model, Constants.ProductPropertyAliases.MasterRelationPropertyAlias, recursive );
            if ( !string.IsNullOrEmpty( masterRelationNodeId ) && UmbracoHelper != null ) {
              rtnValue = GetPropertyValue<T>( UmbracoHelper.TypedContent( masterRelationNodeId ), propertyAlias,
                variant, func );
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

    public virtual string GetSku( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null ) {
      string sku = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.SkuPropertyAlias, variant, recursive: variant == null );

      //If no sku is found - default to umbraco node id
      if ( string.IsNullOrEmpty( sku ) ) {
        sku = model.Id.ToString( CultureInfo.InvariantCulture ) + "_" + variant.VariantId;
      }

      return sku;
    }

    public virtual string GetName( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null ) {
      string name = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.NamePropertyAlias, variant, recursive: variant == null );

      //If no name is found - default to the umbraco node name
      if ( string.IsNullOrEmpty( name ) ) {
        if ( variant != null ) {
          name = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.NamePropertyAlias );
        }
        if ( variant == null ) {
          name = model.Name;
        }
        if ( variant != null ) {
          name += " - " + variant.Name;
        }
      }

      return name;
    }

    public virtual long? GetVatGroupId( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null ) {
      long storeId = GetStoreId( model );
      long? vatGroupId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.VatGroupPropertyAlias, variant );

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

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null ) {
      OriginalUnitPriceCollection prices = new OriginalUnitPriceCollection();

      foreach ( Currency currency in CurrencyService.GetAll( GetStoreId( model ) ) ) {
        prices.Add( new OriginalUnitPrice( GetPropertyValue<string>( model, currency.PricePropertyAlias, variant ).ParseToDecimal() ?? 0M, currency.Id ) );
      }

      return prices;
    }

    public virtual CustomPropertyCollection GetProperties( IPublishedContent model, VariantPublishedContent<IPublishedContent> variant = null ) {
      CustomPropertyCollection properties = new CustomPropertyCollection();

      foreach ( string productPropertyAlias in StoreService.Get( GetStoreId( model ) ).ProductSettings.ProductPropertyAliases ) {
        properties.Add( new CustomProperty( productPropertyAlias, GetPropertyValue<string>( model, productPropertyAlias, variant ) ) { IsReadOnly = true } );
      }

      return properties;
    }

    public virtual ProductSnapshot GetSnapshot( IPublishedContent model, string productIdentifier ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      IPublishedContent content = UmbracoHelper.TypedContent( productIdentifierObj.NodeId );
      long storeId = GetStoreId( content );
      VariantPublishedContent<IPublishedContent> variant = PublishedContentVariantService.Instance.GetVariant( storeId, content, productIdentifierObj.VariantId );
      //We use Clone() because each method should have it's own instance of the navigator - so if they traverse it doesn't affect other methods
      ProductSnapshot snapshot = new ProductSnapshot( storeId, productIdentifier ) {
        Sku = GetSku( model, variant ),
        Name = GetName( model, variant ),
        VatGroupId = GetVatGroupId( model, variant ),
        LanguageId = GetLanguageId( model ),
        OriginalUnitPrices = GetOriginalUnitPrices( model, variant ),
        Properties = GetProperties( model, variant )
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
