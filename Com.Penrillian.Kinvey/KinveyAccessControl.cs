using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Field available in record metadata
    /// </summary>
    // ReSharper disable ClassNeverInstantiated.Global
    // usage implicit
    public class KinveyAccessControl
    {
        // ReSharper disable UnusedMember.Global
        // usages implicit

        /// <summary>
        /// Id of the user who created this record
        /// </summary>
        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        /// <summary>
        /// Is this entity globally readable?
        /// </summary>
        [JsonProperty(PropertyName = "gr")]
        public bool GlobalRead { get; set; }

        /// <summary>
        /// Is this entity globally writable?
        /// </summary>
        [JsonProperty(PropertyName = "gw")]
        public bool GlobalWrite { get; set; }

        /// <summary>
        /// List of usernames who have read access to this entity
        /// </summary>
        [JsonProperty(PropertyName = "r")]
        public ICollection<string> ReadUsers { get; set; }

        /// <summary>
        /// List of usernames who have write access to this entity
        /// </summary>
        [JsonProperty(PropertyName = "w")]
        public ICollection<string> WriteUsers { get; set; }

        /// <summary>
        /// Group Permissions object
        /// </summary>
        [JsonProperty(PropertyName = "groups")]
        public KinveyGroupPermissions GroupPermissions { get; set; }
        // ReSharper restore UnusedMember.Global
    }
    // ReSharper restore ClassNeverInstantiated.Global
}