using System;
using TeaCommerce.Api.Infrastructure.Logging;
using Umbraco.Core.Logging;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Logging {
  public class LoggingProvider : ILoggingProvider {

    public void Log( Exception exception ) {
      Log( exception, null );
    }

    public void Log( string message ) {
      Log( null, message );
    }

    public void Log( Exception exception, string message ) {
      message = message ?? "";

      if ( exception != null ) {
        if ( !string.IsNullOrEmpty( message ) ) {
          message += " - Exception: ";
        }
        message += exception.ToString();
      }
      LogHelper.Error( GetType(), message, exception );
    }

  }
}
