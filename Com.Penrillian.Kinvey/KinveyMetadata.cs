using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Metadata attached to records
    /// </summary>
    public class KinveyMetadata
    {
        /// <summary>
        /// Timestamp of records last modification
        /// </summary>
        [JsonProperty(PropertyName = "lmt")]
        public string LastModifiedTime { get; set; }

        /// <summary>
        /// Timestamp of record creation time
        /// </summary>
        [JsonProperty(PropertyName = "ect")]
        public string EntityCreationTime { get; set; }

        /// <summary>
        /// Status metadata
        /// </summary>
        [JsonProperty("status")]
        public KinveyStatus Status { get; set; }

        /// <summary>
        /// Generated authorization token if record is response to authorization request
        /// </summary>
        [JsonProperty("authtoken")]
        public string AuthToken { get; set; }

        /// <summary>
        /// Email verification meadata available for user records
        /// </summary>
        [JsonProperty(PropertyName = "emailVerification")]
        public KinveyEmailVerification KinveyEmailVerification { get; set; }
    }
}