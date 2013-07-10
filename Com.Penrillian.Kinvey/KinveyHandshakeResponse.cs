using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Response from a handshake with the app server
    /// </summary>
    public class KinveyHandshakeResponse
    {
        private bool Equals(KinveyHandshakeResponse other)
        {
            return string.Equals(Version, other.Version) && string.Equals(Kinvey, other.Kinvey);
        }

        public override bool Equals(object obj)
        {
            // ReSharper disable ConvertIfStatementToReturnStatement
            // readability
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KinveyHandshakeResponse)obj);
            // ReSharper restore ConvertIfStatementToReturnStatement
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Version != null ? Version.GetHashCode() : 0)*397) ^ (Kinvey != null ? Kinvey.GetHashCode() : 0);
            }
        }


        // ReSharper disable MemberCanBePrivate.Global
        // implicit usage
        /// <summary>
        /// The version of the Kinvey API being used
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; internal set; }
        /// <summary>
        /// A greeting message
        /// </summary>
        [JsonProperty(PropertyName = "kinvey")]
        public string Kinvey { get; internal set; }
        // ReSharper restore MemberCanBePrivate.Global
    }
}
