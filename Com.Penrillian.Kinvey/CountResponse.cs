using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    internal class CountResponse
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}