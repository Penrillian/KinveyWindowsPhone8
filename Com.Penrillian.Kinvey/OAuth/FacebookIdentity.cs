using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class FacebookIdentity : SocialIdentity
    {
        public FacebookIdentity()
        {
            Facebook = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string Id
        {
            get { return Facebook["id"]; }
            set { Facebook["id"] = value; }
        }

        [JsonIgnore]
        public string Name
        {
            get { return Facebook["name"]; }
            set { Facebook["name"] = value; }
        }

        [JsonIgnore]
        public string Gender
        {
            get { return Facebook["gender"]; }
            set { Facebook["gender"] = value; }
        }

        [JsonIgnore]
        public string Email
        {
            get { return Facebook["email"]; }
            set { Facebook["email"] = value; }
        }

        [JsonIgnore]
        public string Birthday
        {
            get { return Facebook["birthday"]; }
            set { Facebook["birthday"] = value; }
        }

        [JsonIgnore]
        public string Location
        {
            get { return Facebook["location"]; }
            set { Facebook["location"] = value; }
        }
    }
}