using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.Variant;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class InformationExtractorsConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<ContentProductInformationExtractor>().As<IProductInformationExtractor<IContent, VariantPublishedContent<IContent>>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<ProductInformationExtractorT>().As<IProductInformationExtractor<IPublishedContent, VariantPublishedContent<IPublishedContent>>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentProductInformationExtractor>().As<IPublishedContentProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<ProductInformationExtractor>().As<IProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
