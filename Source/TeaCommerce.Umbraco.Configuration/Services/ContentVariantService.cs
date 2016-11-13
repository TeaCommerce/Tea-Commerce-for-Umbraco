using System.Collections.Generic;
using System.Linq;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using System;
using Umbraco.Core.Logging;

namespace TeaCommerce.Umbraco.Configuration.Services {

  public class ContentVariantService : AVariantService<IContent> {
    public override int GetId( IContent content ) {
      return content.Id;
    }

    public override string GetVariantDataFromContent( long storeId, IContent content, bool onlyValid ) {
      string variantsJson = "";

      if ( content != null ) {
        Store store = StoreService.Instance.Get( storeId );
        if ( content.HasProperty( store.ProductSettings.ProductVariantPropertyAlias ) ) {
          variantsJson = content.GetValue<string>( store.ProductSettings.ProductVariantPropertyAlias );
        } else {
          LogHelper.Debug<PublishedContentVariantService>( "There was no variants in the property \"" + store.ProductSettings.ProductVariantPropertyAlias + "\". Check the " + content.ContentType.Alias + " doc type and the product variant alias setting on the \"" + store.Name + "\" store." );
        }
      }

      return variantsJson;
    }

    public override string GetVariantProductIdentifier( IContent content, VariantPublishedContent<IContent> variant ) {
      return content.Id + "_" + variant.VariantId;
    }
  }
}
