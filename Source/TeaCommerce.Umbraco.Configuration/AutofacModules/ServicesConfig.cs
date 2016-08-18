using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Umbraco.Configuration.Services;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class ServicesConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<CacheService>().As<ICacheService>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<LanguageService>().As<ILanguageService>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PublishedContentVariantService>().As<IVariantService<IPublishedContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<ContentVariantService>().As<IVariantService<IContent>>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
