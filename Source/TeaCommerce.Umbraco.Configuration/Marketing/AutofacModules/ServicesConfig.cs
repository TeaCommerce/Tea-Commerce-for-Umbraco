using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Umbraco.Configuration.Marketing.Services;

namespace TeaCommerce.Umbraco.Configuration.Marketing.AutofacModules {
  public class ServicesConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<ManifestService>().As<IManifestService>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
