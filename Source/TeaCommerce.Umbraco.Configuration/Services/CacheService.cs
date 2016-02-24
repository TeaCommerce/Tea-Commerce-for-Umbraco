using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Autofac;
using TeaCommerce.Api.Dependency;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace TeaCommerce.Umbraco.Configuration.Services {
  public class CacheService : ICacheService {

    private const int MinutesToLive = 30;
    private readonly MemoryCache _cache;
    private List<string> _insertedKeys = new List<string>();

    public CacheService() {
      _cache = MemoryCache.Default;
    }
    public static ICacheService Instance { get { return DependencyContainer.Instance.Resolve<ICacheService>(); } }

    /// <summary>
    /// Gets a value from the application cache. You can only get cached items when the website is not running in debug mode.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    public T Get<T>( string cacheKey ) where T : class {
      if ( !string.IsNullOrEmpty( cacheKey ) ) {
        return (T)_cache.Get( cacheKey );
      }
      return null;
    }

    /// <summary>
    /// Sets a value in the application cache
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cacheValue"></param>
    public void Set( string cacheKey, object cacheValue ) {
      if ( !string.IsNullOrEmpty( cacheKey ) ) {
        Set( cacheKey, cacheValue, TimeSpan.FromMinutes( MinutesToLive ) );
      }
    }

    /// <summary>
    /// Sets a value in the application cache with a specific duration
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cacheValue"></param>
    public void Set( string cacheKey, object cacheValue, TimeSpan cacheDuration ) {
      if ( !string.IsNullOrEmpty( cacheKey ) && cacheValue != null ) {
        _insertedKeys.Add( cacheKey );
        _cache.Set( cacheKey, cacheValue, new CacheItemPolicy { SlidingExpiration = cacheDuration } );
      }
    }

    /// <summary>
    /// Will invalidate a single entry in the application cache
    /// </summary>
    /// <param name="cacheKey"></param>
    public void InvalidateApplicationCache( string cacheKey ) {
      if ( !string.IsNullOrEmpty( cacheKey ) ) {
        _cache.Remove( cacheKey );
      }
    }

    /// <summary>
    /// Will invalidate the entire application cache
    /// </summary>
    public void InvalidateApplicationCache() {
      _insertedKeys.DistinctBy( k => k ).ForEach( InvalidateApplicationCache );
      _insertedKeys = new List<string>();
    }

  }
}
