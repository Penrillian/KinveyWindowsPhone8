using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Record status metadata
    /// </summary>
    public class KinveyStatus
    {
        /// <summary>
        /// The current status of the record
        /// </summary>
        [JsonProperty(PropertyName = "val")]
        public string Value { get; set; }

        /// <summary>
        /// Timestamp of the last status change of the record
        /// </summary>
        [JsonProperty(PropertyName = "lastChange")]
        public string LastChange { get; set; }
    }
}