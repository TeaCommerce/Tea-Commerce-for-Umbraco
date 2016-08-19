using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Services;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Website.Extensions.Ecommerce.Services {

  [SuppressDependency( "TeaCommerce.Umbraco.Configuration.Services.IVariantService`1[[Umbraco.Core.Models.IPublishedContent, Umbraco.Core]]", "TeaCommerce.Umbraco.Configuration" )]
  public class IPublishedContentVariantService : AVariantService<IPublishedContent> {

    public override int GetId( IPublishedContent content ) {
      return content.Id;
    }

    public override string GetVariantDataFromContent( long storeId, IPublishedContent content, bool onlyValid ) {
      string variantsJson = "";

      if ( content != null ) {
        Store store = StoreService.Instance.Get( storeId );
        if ( content.HasProperty( store.ProductSettings.ProductVariantPropertyAlias ) ) {
          variantsJson = content.GetPropertyValue<string>( store.ProductSettings.ProductVariantPropertyAlias );
        } else {
          LogHelper.Debug<IPublishedContentVariantService>( "There was no variants in the property \"" + store.ProductSettings.ProductVariantPropertyAlias + "\". Check the " + content.DocumentTypeAlias + " doc type and the product variant alias setting on the \"" + store.Name + "\" store." );
        }
      }

      return variantsJson;
    }

    public override string GetVariantProductIdentifier( IPublishedContent content, VariantPublishedContent<IPublishedContent> variant ) {
      return content.Id + "_" + variant.VariantId;
    }
  }
}
