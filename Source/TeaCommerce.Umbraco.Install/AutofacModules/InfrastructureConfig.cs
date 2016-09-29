using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Installation;

namespace TeaCommerce.Umbraco.Install.AutofacModules {
  public class InfrastructureConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<Installer>().As<IInstaller>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
