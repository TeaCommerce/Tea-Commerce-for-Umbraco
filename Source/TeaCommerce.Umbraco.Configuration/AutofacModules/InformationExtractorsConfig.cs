using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class InformationExtractorsConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<ContentProductInformationExtractor>().As<IProductInformationExtractor<IContent, VariantPublishedContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<ContentProductInformationExtractor>().As<IProductInformationExtractor<IContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentProductInformationExtractor>().As<IProductInformationExtractor<IPublishedContent, VariantPublishedContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentProductInformationExtractor>().As<IProductInformationExtractor<IPublishedContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentProductInformationExtractor>().As<IPublishedContentProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentProductInformationExtractor>().As<IProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
