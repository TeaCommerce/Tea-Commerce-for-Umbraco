using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class InformationExtractorsConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<ContentProductInformationExtractor>().As<IContentProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentProductInformationExtractor>().As<IPublishedContentProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<ProductInformationExtractor>().As<IProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
