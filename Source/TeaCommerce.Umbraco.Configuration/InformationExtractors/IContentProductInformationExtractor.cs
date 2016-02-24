using System;
using TeaCommerce.Api.Models;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public interface IContentProductInformationExtractor {

    T GetPropertyValue<T>( IContent model, string propertyAlias, string variantGuid = null, Func<IContent, bool> func = null );
    long GetStoreId( IContent model );
    string GetSku( IContent model, string variantGuid = null );
    long? GetVatGroupId( IContent model, string variantGuid = null );
    long? GetLanguageId( IContent model );

  }
}
