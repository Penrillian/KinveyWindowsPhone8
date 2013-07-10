using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    // ReSharper disable ClassNeverInstantiated.Global
    // ReSharper disable UnusedMember.Global
    /// <summary>
    /// Field available in KinveyUser metadata showing email verification status
    /// </summary>
    public class KinveyEmailVerification
    {
        /// <summary>
        /// Current verification status
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; internal set; }

        /// <summary>
        /// Timestamp of last status change
        /// </summary>
        [JsonProperty(PropertyName = "lastStateChangeAt")]
        public string LastStateChange { get; internal set; }

        /// <summary>
        /// Timestamp of last confirmation
        /// </summary>
        [JsonProperty(PropertyName = "lastConfirmedAt")]
        public string LastConfirmed { get; internal set; }

        /// <summary>
        /// Last confirmed email address
        /// </summary>
        [JsonProperty(PropertyName = "emailAddress")]
        public string Email { get; internal set; }
    }
    // ReSharper restore UnusedMember.Global
    // ReSharper restore ClassNeverInstantiated.Global
}