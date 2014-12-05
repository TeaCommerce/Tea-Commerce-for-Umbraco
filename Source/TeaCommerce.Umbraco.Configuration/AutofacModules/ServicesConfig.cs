using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Umbraco.Configuration.Services;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class ServicesConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<LanguageService>().As<ILanguageService>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
