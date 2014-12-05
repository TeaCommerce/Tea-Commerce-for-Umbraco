using Autofac;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Web.PaymentProviders;
using TeaCommerce.Umbraco.Configuration.PaymentProviders;

namespace TeaCommerce.Umbraco.Configuration.AutofacModules {
  public class PaymentProvidersConfig : Module {

    protected override void Load( ContainerBuilder builder ) {
      builder.MustNotBeNull( "builder" );

      builder.RegisterType<PaymentProviderUriResolver>().As<IPaymentProviderUriResolver>().PreserveExistingDefaults().InstancePerLifetimeScope();
      builder.RegisterType<HashSecretProvider>().As<IHashSecretProvider>().PreserveExistingDefaults().InstancePerLifetimeScope();
    }

  }
}
