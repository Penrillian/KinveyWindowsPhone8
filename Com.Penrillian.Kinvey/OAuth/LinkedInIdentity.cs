using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class LinkedInIdentity : SocialIdentity
    {
        public LinkedInIdentity()
        {
            LinkedIn = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string Id
        {
            get { return LinkedIn["id"]; }
            set { LinkedIn["id"] = value; }
        }

        [JsonIgnore]
        public string FirstName
        {
            get { return LinkedIn["firstName"]; }
            set { LinkedIn["firstName"] = value; }
        }

        [JsonIgnore]
        public string LastName
        {
            get { return LinkedIn["lastName"]; }
            set { LinkedIn["lastName"] = value; }
        }

        [JsonIgnore]
        public string Headline
        {
            get { return LinkedIn["headline"]; }
            set { LinkedIn["headline"] = value; }
        }

        [JsonIgnore]
        public string Industry
        {
            get { return LinkedIn["industry"]; }
            set { LinkedIn["industry"] = value; }
        }
    }
}