using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class GooglePlusIdentityToken : SocialIdentityToken
    {
        public GooglePlusIdentityToken()
        {
            GooglePlus = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string AccessToken
        {
            get { return GooglePlus["access_token"]; }
            set { GooglePlus["access_token"] = value; }
        }

        [JsonIgnore]
        public string ExpiresIn
        {
            get { return GooglePlus["expires_in"]; }
            set { GooglePlus["expires_in"] = value; }
        }
    }
}