using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Result of a sign up operation
    /// </summary>
    public class KinveySignUpResult : KinveyUser
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // usage implicit
        /// <summary>
        /// The password for the new user record
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}