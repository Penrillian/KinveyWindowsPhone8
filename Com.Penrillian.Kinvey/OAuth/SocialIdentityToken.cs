using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public abstract class SocialIdentityToken
    {
        [JsonProperty(PropertyName = "facebook", NullValueHandling = NullValueHandling.Ignore)]
        protected Dictionary<string, string> Facebook { get; set; }

        [JsonProperty(PropertyName = "twitter", NullValueHandling = NullValueHandling.Ignore)]
        protected Dictionary<string, string> Twitter { get; set; }

        [JsonProperty(PropertyName = "google", NullValueHandling = NullValueHandling.Ignore)]
        protected Dictionary<string, string> GooglePlus { get; set; }

        [JsonProperty(PropertyName = "linkedin", NullValueHandling = NullValueHandling.Ignore)]
        protected Dictionary<string, string> LinkedIn { get; set; }
    }
}
