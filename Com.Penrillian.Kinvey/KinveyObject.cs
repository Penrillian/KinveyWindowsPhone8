using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Corresponds to an instance of an object in a kinvey collection. Extend this class to
    /// define types in the Kinvey datastore. When extending this class the extending type must
    /// be attributed with the KinveyCollectionAttribute.
    /// </summary>
    public abstract class KinveyObject
    {
        /// <summary>
        /// Unique identifier of an object in a collection
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        /// Metadata associated with this record
        /// </summary>
        [JsonProperty(PropertyName = "_kmd")]
        public KinveyMetadata KinveyMetadata { get; set; }

        /// <summary>
        /// Access control metadata associated with this record
        /// </summary>
        [JsonProperty(PropertyName = "_acl")]
        public KinveyAccessControl KinveyAccessControl { get; set; }
    }
}