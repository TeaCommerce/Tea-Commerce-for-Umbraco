using System;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace TeaCommerce.Umbraco.Configuration.Variant {
  public class VariantPublishedProperty : IPublishedProperty {
    private readonly PublishedPropertyType _propertyType;
    private readonly string _rawValue;

    public VariantPublishedProperty( PublishedPropertyType propertyType, string value ) {
      _propertyType = propertyType;

      _rawValue = value;

    }

    public object DataValue {
      get { return _rawValue; }
    }

    public bool HasValue {
      get { return _rawValue != null || string.IsNullOrWhiteSpace( _rawValue ) == false; }
    }

    public string PropertyTypeAlias {
      get { return _propertyType.PropertyTypeAlias; }
    }

    public object Value {
      get {
        // isPreviewing is true here since we want to preview anyway...
        const bool isPreviewing = true;
        var source = _propertyType.ConvertDataToSource( _rawValue, isPreviewing );
        return _propertyType.ConvertSourceToObject( source, isPreviewing );
      }
    }

    public object XPathValue {
      get { throw new NotImplementedException(); }
    }
  }
}
