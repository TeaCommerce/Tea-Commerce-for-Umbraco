using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Umbraco.Configuration.Services;
using TeaCommerce.Umbraco.Configuration.Variants.Models;
using TeaCommerce.Umbraco.Configuration.Variants.Services;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class ServicesConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<LanguageService>().As<ILanguageService>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentVariantService>().As<IVariantService<IPublishedContent, VariantPublishedContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<ContentVariantService>().As<IVariantService<IContent, VariantPublishedContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
