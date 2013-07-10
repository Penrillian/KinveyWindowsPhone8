using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public abstract class KinveySocialUser<T> : KinveyUser
    {
        [JsonProperty(PropertyName = "socialIdentity")]
        public T SocialIdentity { get; set; }
    }
}