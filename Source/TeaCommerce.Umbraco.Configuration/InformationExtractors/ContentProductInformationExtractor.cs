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
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.Variant;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {

  [SuppressDependency( "TeaCommerce.Api.InformationExtractors.IProductInformationExtractor`2[[Umbraco.Core.Models.IContent, Umbraco.Core],[System.String, mscorlib]]", "TeaCommerce.Api" )]
  public class ContentProductInformationExtractor : IProductInformationExtractor<IContent, string> {

    protected readonly IStoreService StoreService;
    protected readonly ICurrencyService CurrencyService;
    protected readonly IVatGroupService VatGroupService;
    protected readonly IVariantService<IContent> VariantService;

    public static IProductInformationExtractor<IContent, string> Instance { get { return DependencyContainer.Instance.Resolve<IProductInformationExtractor<IContent, string>>(); } }

    public ContentProductInformationExtractor( IStoreService storeService, ICurrencyService currencyService, IVatGroupService vatGroupService, IVariantService<IContent> variantService ) {
      StoreService = storeService;
      CurrencyService = currencyService;
      VatGroupService = vatGroupService;
      VariantService = variantService;
    }

    public virtual T GetPropertyValue<T>( IContent model, string propertyAlias, string variantGuid = null, Func<IContent, bool> func = null ) {
      T rtnValue = default( T );

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        if ( !string.IsNullOrEmpty( variantGuid ) ) {
          IPublishedContent variant = null;
          long storeId = GetStoreId( model );

          variant = VariantService.GetVariant( storeId, model, variantGuid );

          if ( variant != null ) {
            rtnValue = variant.GetPropertyValue<T>( propertyAlias );
          }
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
                variantGuid, func );
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
    
    public virtual long GetStoreId( IContent model ) {
      long? storeId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.StorePropertyAlias );
      if ( storeId == null ) {
        throw new ArgumentException( "The model doesn't have a store id associated with it - remember to add the Tea Commerce store picker to your Umbraco content tree" );
      }

      return storeId.Value;
    }

    public virtual string GetSku( IContent model, string variantGuid = null ) {
      string sku = GetPropertyValue<string>( model, Constants.ProductPropertyAliases.SkuPropertyAlias, variantGuid );

      //If no sku is found - default to umbraco node id
      if ( string.IsNullOrEmpty( sku ) ) {
        sku = model.Id.ToString( CultureInfo.InvariantCulture ) + "_" + variantGuid;
      }

      return sku;
    }

    public virtual long? GetVatGroupId( IContent model, string variantGuid = null ) {
      long storeId = GetStoreId( model );
      long? vatGroupId = GetPropertyValue<long?>( model, Constants.ProductPropertyAliases.VatGroupPropertyAlias, variantGuid );

      //In umbraco a product can have a relation to a delete marked vat group
      if ( vatGroupId != null ) {
        VatGroup vatGroup = VatGroupService.Get( storeId, vatGroupId.Value );
        if ( vatGroup == null || vatGroup.IsDeleted ) {
          vatGroupId = null;
        }
      }

      return vatGroupId;
    }

    public virtual long? GetLanguageId( IContent model ) {
      return LanguageService.Instance.GetLanguageIdByNodePath( model.Path );
    }

    private static bool CheckNullOrEmpty<T>( T value ) {
      if ( typeof( T ) == typeof( string ) )
        return string.IsNullOrEmpty( value as string );

      return value == null || value.Equals( default( T ) );
    }

    public string GetPropertyValue( IContent product, string variantId, string propertyAlias ) {
      return GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias, variantId );
    }

    public string GetName( IContent product, string variantId ) {
      string name = GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias, variantId );

      //If no name is found - default to the umbraco node name
      if ( string.IsNullOrEmpty( name ) ) {
        if ( !string.IsNullOrEmpty( variantId ) ) {
          name = GetPropertyValue<string>( product, Constants.ProductPropertyAliases.NamePropertyAlias );
        }
        if ( string.IsNullOrEmpty( name ) ) {
          name = product.Name;
        }
        if ( !string.IsNullOrEmpty( variantId ) ) {
          long storeId = GetStoreId( product );
          VariantPublishedContent variant = VariantService.GetVariant( storeId, product, variantId, false );
          if ( variant != null ) {
            name += " - " + variant.Name;
          }
        }
      }

      return name;
    }

    public OriginalUnitPriceCollection GetOriginalUnitPrices( IContent product, string variantId ) {
      OriginalUnitPriceCollection prices = new OriginalUnitPriceCollection();

      foreach ( Currency currency in CurrencyService.GetAll( GetStoreId( product ) ) ) {
        prices.Add( new OriginalUnitPrice( GetPropertyValue<string>( product, currency.PricePropertyAlias, variantId ).ParseToDecimal() ?? 0M, currency.Id ) );
      }

      return prices;
    }

    public CustomPropertyCollection GetProperties( IContent product, string variantId ) {
      CustomPropertyCollection properties = new CustomPropertyCollection();

      foreach ( string productPropertyAlias in StoreService.Get( GetStoreId( product ) ).ProductSettings.ProductPropertyAliases ) {
        properties.Add( new CustomProperty( productPropertyAlias, GetPropertyValue<string>( product, productPropertyAlias, variantId ) ) { IsReadOnly = true } );
      }

      return properties;
    }
  }
}
