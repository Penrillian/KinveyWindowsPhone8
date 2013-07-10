using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Metadata describing service errors
    /// </summary>
    public sealed class KinveyError
    {
        /// <summary>
        /// Error status code
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public string Error { get; internal set; }

        /// <summary>
        /// A description of the error
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; internal set; }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // usage implicit
        /// <summary>
        /// Extra debug information about the error
        /// </summary>
        [JsonProperty(PropertyName = "debug")]
        public string DebugInformation { get; internal set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}
