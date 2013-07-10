using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Class representing a user record for the Kinvey app. Users are a special case
    /// and cannot be manipulated using instances of IKinveyService. Instead, they must be 
    /// manipulated using isntances of IKinveyUserService. In order to include extra fields
    /// on a user record, extend this class and use that type as the type parameter on 
    /// instances of IKinveyUserService.
    /// </summary>
    public class KinveyUser
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable MemberCanBePrivate.Global
        // usage is implicit
        /// <summary>
        /// Unique identifier of a user record
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        /// Username for the user record, used while authenticating with IKinveyUserService
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// Email for the user record
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Metadata associated with the user record
        /// </summary>
        [JsonProperty(PropertyName = "_kmd")]
        public KinveyMetadata Metadata { get; set; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedMember.Global

        private bool Equals(KinveyUser other)
        {
            return string.Equals(Username, other.Username);
        }

        public override bool Equals(object obj)
        {
            // ReSharper disable ConvertIfStatementToReturnStatement
            // readability
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KinveyUser)obj);
            // ReSharper restore ConvertIfStatementToReturnStatement
        }

        public override int GetHashCode()
        {
            return (Username != null ? Username.GetHashCode() : 0);
        }
    }
}