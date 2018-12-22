using System;
using System.Collections.Generic;

namespace ViberApiLib
{
    public class UserProfile
    {
        public string Name { get; }

        public string Avatar { get; }

        public string Id { get; }

        public string Country { get; }

        public string Language { get; }

        public string ApiVersion { get; }

        public UserProfile(Dictionary<string, object> dict)
        {
            Id = dict["id"].ToString();
            ApiVersion = dict["api_version"].ToString();

            // There is a case that the following keys are not contained in payload of Viber API.
            Name = dict.ContainsKey("name") ? dict["name"].ToString() : "Nameless User";
            Avatar = dict.ContainsKey("avatar") ? dict["avatar"].ToString() : string.Empty;
            Country = dict.ContainsKey("country") ? dict["country"].ToString() : string.Empty;
            Language = dict.ContainsKey("language") ? dict["language"].ToString() : string.Empty;
        }
    }
}
