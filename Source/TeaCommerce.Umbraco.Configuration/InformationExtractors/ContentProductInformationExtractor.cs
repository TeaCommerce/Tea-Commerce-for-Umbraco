using Autofac;
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Services;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Constants = TeaCommerce.Api.Constants;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {

  public class ContentProductInformationExtractor : IProductInformationExtractor<IContent, VariantPublishedContent>, IProductInformationExtractor<IContent> {

    protected readonly IStoreService StoreService;
    protected readonly ICurrencyService CurrencyService;
    protected readonly IVatGroupService VatGroupService;

    public static IProductInformationExtractor<IContent, VariantPublishedContent> Instance { get { return DependencyContainer.Instance.Resolve<IProductInformationExtractor<IContent, VariantPublishedContent>>(); } }

    #region IProductInformationExtractor<IPublishedContent, VariantPublishedContent>

    public ContentProductInformationExtractor( IStoreService storeService, ICurrencyService currencyService, IVatGroupService vatGroupService ) {
      StoreService = storeService;
      CurrencyService = currencyService;
      VatGroupService = vatGroupService;
    }

    public virtual long GetStoreId( IContent model , VariantPublishedContent variant ) {
      long? storeId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.StorePropertyAlias );
      if ( storeId == null ) {
        throw new ArgumentException( "The model doesn't have a store id associated with it - remember to add the Tea Commerce store picker to your Umbraco content tree" );
      }

      return storeId.Value;
    }

    public virtual string GetSku( IContent model, VariantPublishedContent variant = null ) {
      string sku = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.SkuPropertyAlias, variant );

      //If no sku is found - default to umbraco node id
      if ( string.IsNullOrEmpty( sku ) ) {
        sku = model.Id.ToString( CultureInfo.InvariantCulture ) + ( variant != null ? "_" + variant.VariantIdentifier : "" );
      }

      return sku;
    }

    public virtual string GetName( IContent product, VariantPublishedContent variant = null ) {
      string name = GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias, variant );

      //If no name is found - default to the umbraco node name
      if ( string.IsNullOrEmpty( name ) ) {
        if ( variant != null ) {
          name = GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias );
        }
        if ( string.IsNullOrEmpty( name ) ) {
          name = product.Name;
        }
        if ( variant != null ) {
          name += " - " + variant.Name;
        }
      }

      return name;
    }

    public virtual long? GetVatGroupId( IContent model, VariantPublishedContent variant = null ) {
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

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( IContent product, VariantPublishedContent variant = null ) {
      OriginalUnitPriceCollection prices = new OriginalUnitPriceCollection();

      foreach ( Currency currency in CurrencyService.GetAll( GetStoreId( product ) ) ) {
        prices.Add( new OriginalUnitPrice( GetPropertyValue<string>( product, currency.PricePropertyAlias, variant ).ParseToDecimal() ?? 0M, currency.Id ) );
      }

      return prices;
    }

    protected bool CheckNullOrEmpty<T>( T value ) {
      if ( typeof( T ) == typeof( string ) )
        return string.IsNullOrEmpty( value as string );

      return value == null || value.Equals( default( T ) );
    }

    public virtual long? GetLanguageId( IContent model, VariantPublishedContent variant ) {
      return LanguageService.Instance.GetLanguageIdByNodePath( model.Path );
    }

    public virtual CustomPropertyCollection GetProperties( IContent product, VariantPublishedContent variant = null ) {
      CustomPropertyCollection properties = new CustomPropertyCollection();

      foreach ( string productPropertyAlias in StoreService.Get( GetStoreId( product ) ).ProductSettings.ProductPropertyAliases ) {
        properties.Add( new CustomProperty( productPropertyAlias, GetPropertyValue<string>( product, productPropertyAlias, variant ) ) { IsReadOnly = true } );
      }

      return properties;
    }

    #endregion

    #region IProductInformationExtractor<IPublishedContent>

    public long GetStoreId( IContent product ) {
      return GetStoreId( product, null );
    }

    public string GetSku( IContent product ) {
      return GetSku( product, null );
    }

    public string GetName( IContent product ) {
      return GetName( product, null );
    }

    public long? GetVatGroupId( IContent product ) {
      return GetVatGroupId( product, null );
    }

    public OriginalUnitPriceCollection GetOriginalUnitPrices( IContent product ) {
      return GetOriginalUnitPrices( product, null );
    }

    public long? GetLanguageId( IContent product ) {
      return GetLanguageId( product, null );
    }

    public CustomPropertyCollection GetProperties( IContent product ) {
      return GetProperties( product, null );
    }

    public string GetPropertyValue( IContent product, string propertyAlias ) {
      return GetPropertyValue( product, propertyAlias, null );
    }

    #endregion

    public virtual string GetPropertyValue( IContent product, string propertyAlias, VariantPublishedContent variant = null ) {
      return GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias, variant );
    }

    public virtual T GetPropertyValue<T>( IContent model, string propertyAlias, VariantPublishedContent variant = null, Func<IContent, bool> func = null ) {
      T rtnValue = default( T );

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        if ( variant != null ) {
          rtnValue = variant.GetPropertyValue<T>( propertyAlias );
        }
        if ( CheckNullOrEmpty( rtnValue ) ) {
          //Check if this node or ancestor has it
          IContent currentNode = func != null ? model.Ancestors().FirstOrDefault( func ) : model;
          if ( currentNode != null ) {
            rtnValue = GetPropertyValueInternal<T>( currentNode, propertyAlias, func == null );
          }

          //Check if we found the value
          if ( CheckNullOrEmpty( rtnValue ) ) {

            //Check if we can find a master relation
            string masterRelationNodeIdStr = GetPropertyValueInternal<string>( model, Constants.ProductPropertyAliases.MasterRelationPropertyAlias, true );
            int masterRelationNodeId = 0;
            if ( !string.IsNullOrEmpty( masterRelationNodeIdStr ) && int.TryParse( masterRelationNodeIdStr, out masterRelationNodeId ) ) {
              rtnValue = GetPropertyValue<T>( ApplicationContext.Current.Services.ContentService.GetById( masterRelationNodeId ), propertyAlias,
                variant, func );
            }
          }

        }
      }

      return rtnValue;
    }

    protected virtual T GetPropertyValueInternal<T>( IContent content, string propertyAlias, bool recursive ) {
      T rtnValue = default( T );

      if ( content != null && !string.IsNullOrEmpty( propertyAlias ) ) {

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

      return rtnValue;
    }
  }
}
