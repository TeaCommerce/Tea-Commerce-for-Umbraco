using Newtonsoft.Json;

namespace TeaCommerce.Umbraco.Application.Caching
{
    public class TeaCommerceCacheRefresherPayload<TId>
    {
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("storeId")]
        public long StoreId { get; set; }

        [JsonProperty("id")]
        public TId Id { get; set; }

        [JsonProperty("action")]
        public CacheRefresherAction Action { get; set; }
    }

    public enum CacheRefresherAction
    {
        Created,
        Updated,
        Deleted
    }
}