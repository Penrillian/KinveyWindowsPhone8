using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class FacebookIdentityToken : SocialIdentityToken
    {
        public FacebookIdentityToken()
        {
            Facebook = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string AccessToken
        {
            get { return Facebook["access_token"]; }
            set { Facebook["access_token"] = value; }
        }

        [JsonIgnore]
        public string Expires
        {
            get { return Facebook["expires"]; }
            set { Facebook["expires"] = value; }
        }
    }
}