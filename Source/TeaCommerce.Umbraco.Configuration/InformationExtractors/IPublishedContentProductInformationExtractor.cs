using System;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public interface IPublishedContentProductInformationExtractor {

    T GetPropertyValue<T>( IPublishedContent model, string propertyAlias, VariantPublishedContent variant = null, Func<IPublishedContent, bool> func = null, bool recursive = true );
    
  }
}
