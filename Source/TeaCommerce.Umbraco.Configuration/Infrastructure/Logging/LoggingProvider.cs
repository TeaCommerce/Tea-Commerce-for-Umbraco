using System;
using TeaCommerce.Api.Infrastructure.Logging;
using Umbraco.Core.Logging;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Logging {
  public class LoggingProvider : ILoggingProvider {

    public void Error<T>( string message, Exception exception ) {
      LogHelper.Error<T>( message, exception );
    }

    public void Info<T>( string message ) {
      LogHelper.Info<T>( message );
    }

    public void Warn<T>( string message ) {
      LogHelper.Warn<T>( message );
    }
  }
}
