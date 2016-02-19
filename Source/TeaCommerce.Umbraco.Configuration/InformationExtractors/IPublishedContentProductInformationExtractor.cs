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

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class IPublishedContentProductInformationExtractor : IIPublishedContentProductInformationExtractor {

    internal UmbracoHelper UmbracoHelper;

    protected readonly IStoreService StoreService;
    protected readonly ICurrencyService CurrencyService;
    protected readonly IVatGroupService VatGroupService;

    public static IIPublishedContentProductInformationExtractor Instance { get { return DependencyContainer.Instance.Resolve<IIPublishedContentProductInformationExtractor>(); } }

    public IPublishedContentProductInformationExtractor( IStoreService storeService, ICurrencyService currencyService, IVatGroupService vatGroupService ) {
      StoreService = storeService;
      CurrencyService = currencyService;
      VatGroupService = vatGroupService;
      UmbracoHelper = new UmbracoHelper( UmbracoContext.Current );
    }

    public virtual T GetPropertyValue<T>( IPublishedContent model, string propertyAlias, string variantGuid = null, Func<IPublishedContent, bool> func = null, bool useCachedInformation = true ) {
      T rtnValue = default( T );

      //TODO: Håndter at modellen er null hvis noden ikke er publiseret, men så har vi jo ikke noget id!!
      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        if ( !string.IsNullOrEmpty( variantGuid ) ) {
          IPublishedContent variant = null;

          if ( useCachedInformation ) {
            variant = VariantService.Instance.GetVariant( model, variantGuid );
          } else {
            IContent content = ApplicationContext.Current.Services.ContentService.GetById( model.Id );
            variant = VariantService.Instance.GetVariant( content, variantGuid );
          }
          if ( variant != null ) {
            rtnValue = variant.GetPropertyValue<T>( propertyAlias );
          }
        }
        if ( CheckNullOrEmpty( rtnValue ) ) {
          //Check if this node or ancestor has it
          IPublishedContent currentNode = func != null ? model.AncestorsOrSelf().FirstOrDefault( func ) : model;
          if ( currentNode != null ) {
            rtnValue = GetPropertyValueInternal<T>( currentNode, propertyAlias, func == null, variantGuid, useCachedInformation );
          }

          //Check if we found the value
          if ( CheckNullOrEmpty( rtnValue ) ) {

            //Check if we can find a master relation
            string masterRelationNodeId = GetPropertyValueInternal<string>( model, Constants.ProductPropertyAliases.MasterRelationPropertyAlias, true, variantGuid, useCachedInformation );
            if ( !string.IsNullOrEmpty( masterRelationNodeId ) ) {
              rtnValue = GetPropertyValue<T>( UmbracoHelper.TypedContent( masterRelationNodeId ), propertyAlias,
                variantGuid, func,
                useCachedInformation );
            }
          }

        }
      }

      return rtnValue;
    }

    protected virtual T GetPropertyValueInternal<T>( IPublishedContent model, string propertyAlias, bool recursive, string variantGuid = null, bool useCachedInformation = true ) {
      T rtnValue = default( T );

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {

        if ( useCachedInformation ) {
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
        } else {
          IContent content = ApplicationContext.Current.Services.ContentService.GetById( model.Id );
          if ( !recursive ) {
            rtnValue = content.GetValue<T>( propertyAlias );
          } else {
            //We need to go recursive
            IContent tempModel = content;
            T tempProperty = default( T );
            try {
              tempProperty = tempModel.GetValue<T>( propertyAlias );
            } catch { }
            if ( !CheckNullOrEmpty( tempProperty ) ) {
              rtnValue = tempProperty;
            }

            while ( CheckNullOrEmpty( rtnValue ) && tempModel != null && tempModel.Id > 0 ) {
              tempModel = tempModel.Parent();
              if ( tempModel != null ) {
                try {
                  tempProperty = tempModel.GetValue<T>( propertyAlias );
                } catch { }
                if ( !CheckNullOrEmpty( tempProperty ) ) {
                  rtnValue = tempProperty;
                }
              }
            }
          }
        }
      }

      return rtnValue;
    }



    public virtual long GetStoreId( IPublishedContent model, bool useCachedInformation = true ) {
      long? storeId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.StorePropertyAlias, useCachedInformation: useCachedInformation );
      if ( storeId == null ) {
        throw new ArgumentException( "The model doesn't have a store id associated with it - remember to add the Tea Commerce store picker to your Umbraco content tree" );
      }

      return storeId.Value;
    }

    public virtual string GetSku( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true ) {
      string sku = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.SkuPropertyAlias, useCachedInformation: useCachedInformation );

      //If no sku is found - default to umbraco node id
      if ( string.IsNullOrEmpty( sku ) ) {
        sku = model.Id.ToString( CultureInfo.InvariantCulture ); //todo: plus variant guid
      }

      return sku;
    }

    public virtual string GetName( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true ) {
      string name = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.NamePropertyAlias, useCachedInformation: useCachedInformation );

      //If no name is found - default to the umbraco node name
      if ( string.IsNullOrEmpty( name ) ) {
        name = model.Name;
      }

      return name;
    }

    public virtual long? GetVatGroupId( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true ) {
      long storeId = GetStoreId( model, useCachedInformation );
      long? vatGroupId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.VatGroupPropertyAlias, useCachedInformation: useCachedInformation );

      //In umbraco a product can have a relation to a delete marked vat group
      if ( vatGroupId != null ) {
        VatGroup vatGroup = VatGroupService.Get( storeId, vatGroupId.Value );
        if ( vatGroup == null || vatGroup.IsDeleted ) {
          vatGroupId = null;
        }
      }

      return vatGroupId;
    }

    public virtual long? GetLanguageId( IPublishedContent model, bool useCachedInformation = true ) {
      return LanguageService.Instance.GetLanguageIdByNodePath( model.Path );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true ) {
      OriginalUnitPriceCollection prices = new OriginalUnitPriceCollection();

      foreach ( Currency currency in CurrencyService.GetAll( GetStoreId( model, useCachedInformation ) ) ) {
        prices.Add( new OriginalUnitPrice( GetPropertyValue<string>( model, currency.PricePropertyAlias,  variantGuid, useCachedInformation: useCachedInformation ).ParseToDecimal() ?? 0M, currency.Id ) );
      }

      return prices;
    }

    public virtual CustomPropertyCollection GetProperties( IPublishedContent model, string variantGuid = null, bool useCachedInformation = true ) {
      CustomPropertyCollection properties = new CustomPropertyCollection();

      foreach ( string productPropertyAlias in StoreService.Get( GetStoreId( model, useCachedInformation ) ).ProductSettings.ProductPropertyAliases ) {
        properties.Add( new CustomProperty( productPropertyAlias, GetPropertyValue<string>( model, productPropertyAlias, useCachedInformation: useCachedInformation ) ) { IsReadOnly = true } );
      }

      return properties;
    }

    public virtual ProductSnapshot GetSnapshot( IPublishedContent model, string productIdentifier, bool useCachedInformation = true ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      //We use Clone() because each method should have it's own instance of the navigator - so if they traverse it doesn't affect other methods
      ProductSnapshot snapshot = new ProductSnapshot( GetStoreId( model, useCachedInformation ), productIdentifier ) {
        Sku = GetSku( model, productIdentifierObj.VariantId, useCachedInformation ),
        Name = GetName( model, productIdentifierObj.VariantId, useCachedInformation ),
        VatGroupId = GetVatGroupId( model, productIdentifierObj.VariantId, useCachedInformation ),
        LanguageId = GetLanguageId( model, useCachedInformation ),
        OriginalUnitPrices = GetOriginalUnitPrices( model, productIdentifierObj.VariantId, useCachedInformation ),
        Properties = GetProperties( model, productIdentifierObj.VariantId, useCachedInformation )
      };

      return snapshot;
    }

    public virtual bool HasAccess( long storeId, IPublishedContent model, bool useCachedInformation = true ) {
      return storeId == GetStoreId( model ) && library.HasAccess( model.Id, model.Path );
    }

    private static bool CheckNullOrEmpty<T>( T value ) {
      if ( typeof( T ) == typeof( string ) )
        return string.IsNullOrEmpty( value as string );

      return value == null || value.Equals( default( T ) );
    }
  }
}
