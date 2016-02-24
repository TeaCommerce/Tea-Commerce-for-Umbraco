using System;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public interface ICacheService {

    T Get<T>( string cacheKey ) where T : class;
    void Set( string cacheKey, object cacheValue );
    void Set( string cacheKey, object cacheValue, TimeSpan cacheDuration );
    void InvalidateApplicationCache( string cacheKey );
    void InvalidateApplicationCache();

  }
}
