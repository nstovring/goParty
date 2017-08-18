using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Models
{
    public class UserInfo
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset createdAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset updatedAt { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        [JsonProperty("deleted")]
        public bool deleted { get; set; }

        [JsonProperty("userId")]
        public string userId { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("age_range")]
        public Age_Range age_range { get; set; }

        [JsonProperty("picture")]
        public Picture picture { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }


        [JsonObject(Title = "age_range")]
        public class Age_Range
        {
            [JsonProperty("min")]
            public int min { get; set; }
        }

        [JsonObject(Title = "picture")]
        public class Picture
        {
            [JsonProperty("data")]
            public Data data;
            [JsonObject(Title = "data")]
            public class Data
            {
                [JsonProperty("is_silhouette")]
                public bool is_silhouette { get; set; }
                [JsonProperty("url")]
                public string url { get; set; }
            }
        }
    }
}
