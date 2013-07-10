using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class TwitterIdentityToken : SocialIdentityToken
    {
        public TwitterIdentityToken()
        {
            Twitter = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string AccessToken
        {
            get { return Twitter["access_token"]; }
            set { Twitter["access_token"] = value; }
        }

        [JsonIgnore]
        public string AccessTokenSecret
        {
            get { return Twitter["access_token_secret"]; }
            set { Twitter["access_token_secret"] = value; }
        }

        [JsonIgnore]
        public string ConsumerKey
        {
            get { return Twitter["consumer_key"]; }
            set { Twitter["consumer_key"] = value; }
        }

        [JsonIgnore]
        public string ConsumerSecret
        {
            get { return Twitter["consumer_secret"]; }
            set { Twitter["consumer_secret"] = value; }
        }
    }
}