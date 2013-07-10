using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.OAuth
{
    public sealed class GooglePlusIdentity : SocialIdentity
    {
        public GooglePlusIdentity()
        {
            GooglePlus = new Dictionary<string, string>();
        }

        [JsonIgnore]
        public string Id
        {
            get { return GooglePlus["id"]; }
            set { GooglePlus["id"] = value; }
        }

        [JsonIgnore]
        public string Email
        {
            get { return GooglePlus["email"]; }
            set { GooglePlus["email"] = value; }
        }

        [JsonIgnore]
        public bool VerifiedEmail
        {
            get { return Boolean.Parse(GooglePlus["verified_email"]); }
            set { GooglePlus["verified_email"] = value.ToString(); }
        }

        [JsonIgnore]
        public string Name
        {
            get { return GooglePlus["name"]; }
            set { GooglePlus["name"] = value; }
        }

        [JsonIgnore]
        public string GivenName
        {
            get { return GooglePlus["given_name"]; }
            set { GooglePlus["given_name"] = value; }
        }

        [JsonIgnore]
        public string FamilyName
        {
            get { return GooglePlus["family_name"]; }
            set { GooglePlus["family_name"] = value; }
        }

        [JsonIgnore]
        public string Link
        {
            get { return GooglePlus["link"]; }
            set { GooglePlus["link"] = value; }
        }

        [JsonIgnore]
        public string Gender
        {
            get { return GooglePlus["gender"]; }
            set { GooglePlus["gender"] = value; }
        }

        [JsonIgnore]
        public string Birthday
        {
            get { return GooglePlus["birthday"]; }
            set { GooglePlus["birthday"] = value; }
        }

        [JsonIgnore]
        public string Locale
        {
            get { return GooglePlus["locale"]; }
            set { GooglePlus["locale"] = value; }
        }
    }
}