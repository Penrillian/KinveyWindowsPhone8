using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    // ReSharper disable ClassNeverInstantiated.Global
    /// <summary>
    /// Group Permissions field on Access Control object
    /// </summary>
    public class KinveyGroupPermissions
    {
        // ReSharper disable UnusedMember.Global
        // usage implicit
        [JsonProperty(PropertyName = "r")]
        public ICollection<string> ReadGroups { get; set; }

        [JsonProperty(PropertyName = "w")]
        public ICollection<string> WriteGroups { get; set; }
        // ReSharper restore UnusedMember.Global
    }
    // ReSharper restore ClassNeverInstantiated.Global
}