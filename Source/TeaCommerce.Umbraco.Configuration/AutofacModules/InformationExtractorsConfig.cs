using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class InformationExtractorsConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<IPublishedContentProductInformationExtractor>().As<IIPublishedContentProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<ProductInformationExtractor>().As<IProductInformationExtractor>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
