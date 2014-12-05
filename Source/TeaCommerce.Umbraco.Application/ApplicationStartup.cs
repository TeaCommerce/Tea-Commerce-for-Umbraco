using System.Globalization;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace TeaCommerce.Umbraco.Application {

  public class ApplicationStartup : ApplicationEventHandler {

    protected override void ApplicationStarted( UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext ) {
      ContentService.Copying += ContentService_Copying;
    }

    void ContentService_Copying( IContentService sender, CopyEventArgs<IContent> e ) {
      Property masterRelationProperty = e.Copy.Properties.SingleOrDefault( p => p.Alias == Api.Constants.ProductPropertyAliases.MasterRelationPropertyAlias );

      if ( masterRelationProperty == null || ( masterRelationProperty.Value != null && !string.IsNullOrEmpty( masterRelationProperty.Value.ToString() ) ) ) {
        return;
      }

      //Delete all property data
      foreach ( Property property in e.Copy.Properties ) {
        property.Value = null;
      }

      masterRelationProperty.Value = e.Original.Id.ToString( CultureInfo.InvariantCulture );
    }

  }
}