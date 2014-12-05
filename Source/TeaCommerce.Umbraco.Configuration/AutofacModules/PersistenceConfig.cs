using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Persistence;
using TeaCommerce.Umbraco.Configuration.Persistence;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class PersistenceConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
