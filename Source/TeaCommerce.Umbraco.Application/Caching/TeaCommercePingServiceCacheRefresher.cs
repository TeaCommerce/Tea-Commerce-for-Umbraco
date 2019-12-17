//using System;

//namespace TeaCommerce.Umbraco.Application.Caching
//{
//    // TODO: Need to figure out how to handle this one
//    // I think the ping service should probably only run
//    // on the master instance
//    public class TeaCommercePingServiceCacheRefresher : TeaCommerceCacheRefresherBase<TeaCommercePingServiceCacheRefresher>
//    {
//        public override Guid UniqueIdentifier => Constants.DistributedCache.PingServiceCacheRefresherGuid;

//        public override string Name => "Tea Commerce Ping Service cache refresher";

//        protected override TeaCommercePingServiceCacheRefresher Instance => this;

//        public override void Refresh(int Id)
//        {
//            // ClearCache(Id);
//            base.Refresh(Id);
//        }

//        public override void RefreshAll()
//        {
//            throw new NotImplementedException();
//        }

//        public override void Remove(int Id)
//        {
//            // ClearCache(Id);
//            base.Remove(Id);
//        }

//        protected void ClearCache(string domain)
//        {
//            CacheService.Invalidate($"PingService-{domain}");
//        }
//    }
//}