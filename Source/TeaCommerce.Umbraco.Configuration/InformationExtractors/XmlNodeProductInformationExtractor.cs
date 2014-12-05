using Autofac;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Services;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using Constants = TeaCommerce.Api.Constants;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class XmlNodeProductInformationExtractor : IXmlNodeProductInformationExtractor {

    protected readonly IStoreService StoreService;
    protected readonly ICurrencyService CurrencyService;
    protected readonly IVatGroupService VatGroupService;

    public static IXmlNodeProductInformationExtractor Instance { get { return DependencyContainer.Instance.Resolve<IXmlNodeProductInformationExtractor>(); } }

    public XmlNodeProductInformationExtractor( IStoreService storeService, ICurrencyService currencyService, IVatGroupService vatGroupService ) {
      StoreService = storeService;
      CurrencyService = currencyService;
      VatGroupService = vatGroupService;
    }

    public virtual string GetPropertyValue( XPathNavigator model, string propertyAlias, string selector = null, bool useCachedInformation = true ) {
      string propertyValue = "";

      XPathNavigator xmlProperty = GetXmlPropertyValue( model, propertyAlias, selector, useCachedInformation );
      if ( xmlProperty != null ) {
        propertyValue = xmlProperty.Value;
      }

      return propertyValue;
    }

    public virtual XPathNavigator GetXmlPropertyValue( XPathNavigator model, string propertyAlias, string selector = null, bool useCachedInformation = true ) {
      //Check if this node or ancestor has it
      XPathNavigator xmlProperty = GetXmlPropertyValueInternal( model, propertyAlias, selector, useCachedInformation );

      //Check if we found the value
      if ( xmlProperty == null ) {

        //Check if we can find a master relation
        XPathNavigator masterRelationNodeId = GetXmlPropertyValueInternal( model, Constants.ProductPropertyAliases.MasterRelationPropertyAlias, useCachedInformation: useCachedInformation );
        if ( masterRelationNodeId != null ) {
          XPathNodeIterator masterRelation = library.GetXmlNodeById( masterRelationNodeId.Value );
          if ( masterRelation != null ) {
            xmlProperty = GetXmlPropertyValue( masterRelation.Current, propertyAlias, selector, useCachedInformation );
          }
        }

      }

      return xmlProperty;
    }

    protected virtual XPathNavigator GetXmlPropertyValueInternal( XPathNavigator model, string propertyAlias, string selector = null, bool useCachedInformation = true ) {
      XPathNavigator navigator = null;

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        string propertySelector = !UmbracoSettings.UseLegacyXmlSchema || propertyAlias.IndexOf( '@' ) == 0 ? propertyAlias : "data[@alias = '" + propertyAlias + "']";
        string xpath = string.Format( "./ancestor-or-self::* [string({0}) != ''{1}][1]/{0}", propertySelector, !string.IsNullOrEmpty( selector ) ? " and " + selector : "" );

        bool nodePublished = model.SelectSingleNode( "./error" ) == null;

        if ( useCachedInformation && nodePublished ) {
          navigator = model.SelectSingleNode( xpath );
        } else {
          //The node isnt published or we want to use uncached info (saving a Umbraco node) - we try and fetch the info one node at a time until we find a published node
          try {
            IContent content = ApplicationContext.Current.Services.ContentService.GetById( int.Parse( !nodePublished ? Regex.Match( model.Value, @"\d+" ).Value : model.SelectSingleNode( "@id" ).Value ) );

            if ( !propertyAlias.StartsWith( "@" ) ) {
              Property property = content.Properties.SingleOrDefault( p => p.Alias == propertyAlias );
              if ( property != null ) {
                string propertyValue = property.Value.ToString();
                if ( !string.IsNullOrEmpty( propertyValue ) ) {
                  navigator = ( propertyValue.StartsWith( "<" ) && propertyValue.EndsWith( ">" ) ? XElement.Parse( property.Value.ToString() ) : new XElement( "value", propertyValue ) ).CreateNavigator().SelectSingleNode( "/" );
                }
              }
            } else {
              if ( propertyAlias == "@id" ) {
                navigator = new XElement( "value", content.Id ).CreateNavigator().SelectSingleNode( "/" );
              } else if ( propertyAlias == "@nodeName" ) {
                navigator = new XElement( "value", content.Name ).CreateNavigator().SelectSingleNode( "/" );
              } else if ( propertyAlias == "@path" ) {
                navigator = new XElement( "value", content.Path ).CreateNavigator().SelectSingleNode( "/" );
              }
            }

            if ( navigator == null && content.ParentId != -1 ) {
              navigator = GetXmlPropertyValueInternal( library.GetXmlNodeById( content.ParentId.ToString( CultureInfo.InvariantCulture ) ).Current, propertyAlias, selector, useCachedInformation );
            }
          } catch ( Exception ) {
          }
        }
      }

      return navigator;
    }

    public virtual long GetStoreId( XPathNavigator model, bool useCachedInformation = true ) {
      long? storeId = GetPropertyValue( model, Constants.ProductPropertyAliases.StorePropertyAlias, useCachedInformation: useCachedInformation ).TryParse<long>();
      if ( storeId == null ) {
        throw new ArgumentException( "The model doesn't have a store id associated with it - remember to add the Tea Commerce store picker to your Umbraco content tree" );
      }

      return storeId.Value;
    }

    public virtual string GetSku( XPathNavigator model, bool useCachedInformation = true ) {
      string sku = GetPropertyValue( model, Constants.ProductPropertyAliases.SkuPropertyAlias, useCachedInformation: useCachedInformation );

      //If no sku is found - default to umbraco node id
      if ( string.IsNullOrEmpty( sku ) ) {
        sku = GetPropertyValue( model, "@id", useCachedInformation: useCachedInformation );
      }

      return sku;
    }

    public virtual string GetName( XPathNavigator model, bool useCachedInformation = true ) {
      string name = GetPropertyValue( model, Constants.ProductPropertyAliases.NamePropertyAlias, useCachedInformation: useCachedInformation );

      //If no name is found - default to the umbraco node name
      if ( string.IsNullOrEmpty( name ) ) {
        name = GetPropertyValue( model, "@nodeName", useCachedInformation: useCachedInformation );
      }

      return name;
    }

    public virtual long? GetVatGroupId( XPathNavigator model, bool useCachedInformation = true ) {
      long storeId = GetStoreId( model, useCachedInformation );
      long? vatGroupId = GetPropertyValue( model, Constants.ProductPropertyAliases.VatGroupPropertyAlias, useCachedInformation: useCachedInformation ).TryParse<long>();

      //In umbraco a product can have a relation to a delete marked vat group
      if ( vatGroupId != null ) {
        VatGroup vatGroup = VatGroupService.Get( storeId, vatGroupId.Value );
        if ( vatGroup == null || vatGroup.IsDeleted ) {
          vatGroupId = null;
        }
      }

      return vatGroupId;
    }

    public virtual long? GetLanguageId( XPathNavigator model, bool useCachedInformation = true ) {
      return LanguageService.Instance.GetLanguageIdByNodePath( GetPropertyValue( model, "@path", useCachedInformation: useCachedInformation ) );
    }

    public virtual OriginalUnitPriceCollection GetOriginalUnitPrices( XPathNavigator model, bool useCachedInformation = true ) {
      OriginalUnitPriceCollection prices = new OriginalUnitPriceCollection();

      foreach ( Currency currency in CurrencyService.GetAll( GetStoreId( model, useCachedInformation ) ) ) {
        prices.Add( new OriginalUnitPrice( GetPropertyValue( model, currency.PricePropertyAlias, useCachedInformation: useCachedInformation ).ParseToDecimal() ?? 0M, currency.Id ) );
      }

      return prices;
    }

    public virtual CustomPropertyCollection GetProperties( XPathNavigator model, bool useCachedInformation = true ) {
      CustomPropertyCollection properties = new CustomPropertyCollection();

      foreach ( string productPropertyAlias in StoreService.Get( GetStoreId( model, useCachedInformation ) ).ProductSettings.ProductPropertyAliases ) {
        properties.Add( new CustomProperty( productPropertyAlias, GetPropertyValue( model, productPropertyAlias, useCachedInformation: useCachedInformation ) ) { IsReadOnly = true } );
      }

      return properties;
    }

    public virtual ProductSnapshot GetSnapshot( XPathNavigator model, string productIdentifier, bool useCachedInformation = true ) {
      //We use Clone() because each method should have it's own instance of the navigator - so if they traverse it doesn't affect other methods
      ProductSnapshot snapshot = new ProductSnapshot( GetStoreId( model.Clone(), useCachedInformation ), productIdentifier ) {
        Sku = GetSku( model.Clone(), useCachedInformation ),
        Name = GetName( model.Clone(), useCachedInformation ),
        VatGroupId = GetVatGroupId( model.Clone(), useCachedInformation ),
        LanguageId = GetLanguageId( model.Clone(), useCachedInformation ),
        OriginalUnitPrices = GetOriginalUnitPrices( model.Clone(), useCachedInformation ),
        Properties = GetProperties( model.Clone(), useCachedInformation )
      };

      return snapshot;
    }

    public virtual bool HasAccess( long storeId, XPathNavigator model, bool useCachedInformation = true ) {
      return storeId == GetStoreId( model ) && library.HasAccess( int.Parse( GetPropertyValue( model, "@id", useCachedInformation: useCachedInformation ) ), GetPropertyValue( model, "@path", useCachedInformation: useCachedInformation ) );
    }
  }
}
