using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    internal class LogInDetails
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // usage implicit
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}
