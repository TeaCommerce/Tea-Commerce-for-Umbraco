using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Umbraco.Configuration.Services;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class ServicesConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<CacheService>().As<ICacheService>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<LanguageService>().As<ILanguageService>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<VariantService>().As<IVariantService>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
