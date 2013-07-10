using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class LinkedInIdentityToken : SocialIdentityToken
    {
        public LinkedInIdentityToken()
        {
            LinkedIn = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string AccessToken
        {
            get { return LinkedIn["access_token"]; }
            set { LinkedIn["access_token"] = value; }
        }

        [JsonIgnore]
        public string AccessTokenSecret
        {
            get { return LinkedIn["access_token_secret"]; }
            set { LinkedIn["access_token_secret"] = value; }
        }

        [JsonIgnore]
        public string ConsumerKey
        {
            get { return LinkedIn["consumer_key"]; }
            set { LinkedIn["consumer_key"] = value; }
        }

        [JsonIgnore]
        public string ConsumerSecret
        {
            get { return LinkedIn["consumer_secret"]; }
            set { LinkedIn["consumer_secret"] = value; }
        }
    }
}