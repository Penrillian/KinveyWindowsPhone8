using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class TwitterIdendity : SocialIdentity
    {
        public TwitterIdendity()
        {
            Twitter = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string Id
        {
            get { return Twitter["id"]; }
            set { Twitter["id"] = value; }
        }

        [JsonIgnore]
        public string Name
        {
            get { return Twitter["name"]; }
            set { Twitter["name"] = value; }
        }

        [JsonIgnore]
        public string Language
        {
            get { return Twitter["lang"]; }
            set { Twitter["lang"] = value; }
        }

        [JsonIgnore]
        public string ScreenName
        {
            get { return Twitter["screen_name"]; }
            set { Twitter["screen_name"] = value; }
        }

        [JsonIgnore]
        public string Location
        {
            get { return Twitter["location"]; }
            set { Twitter["location"] = value; }
        }

        [JsonIgnore]
        public string Url
        {
            get { return Twitter["url"]; }
            set { Twitter["url"] = value; }
        }

        [JsonIgnore]
        public string Description
        {
            get { return Twitter["description"]; }
            set { Twitter["description"] = value; }
        }
    }
}