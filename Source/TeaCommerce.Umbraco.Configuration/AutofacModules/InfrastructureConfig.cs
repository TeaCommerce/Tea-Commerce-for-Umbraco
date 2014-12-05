using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Installation;
using TeaCommerce.Api.Infrastructure.Logging;
using TeaCommerce.Api.Infrastructure.Ping;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Infrastructure.Templating;
using TeaCommerce.Umbraco.Configuration.Infrastructure.Installation;
using TeaCommerce.Umbraco.Configuration.Infrastructure.Logging;
using TeaCommerce.Umbraco.Configuration.Infrastructure.Ping;
using TeaCommerce.Umbraco.Configuration.Infrastructure.Security;
using TeaCommerce.Umbraco.Configuration.Infrastructure.Templating;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class InfrastructureConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<Installer>().As<IInstaller>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<LoggingProvider>().As<ILoggingProvider>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PingDataProvider>().As<IPingDataProvider>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<PermissionProvider>().As<IPermissionProvider>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<TemplateRenderer>().As<ITemplateRenderer>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<TemplateFileLocator>().As<ITemplateFileLocator>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
