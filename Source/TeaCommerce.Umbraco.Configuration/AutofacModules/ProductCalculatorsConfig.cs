using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Api.PriceCalculators;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.PriceCalculators;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class ProductCalculatorsConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      //builder.RegisterType<ContentProductInformationExtractor>().As<IContentProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
      //builder.RegisterType<PublishedContentProductInformationExtractor>().As<IPublishedContentProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentProductCalculator>().As<IProductCalculator<IPublishedContent, string>>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
