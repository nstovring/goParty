using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Spatial;
namespace goParty.Models
{
    enum PartyType { Club, Home, Bar, PopUp, Street }

    [JsonObject(Title = "PartyDetails")]
    public class PartyDetails
    {
        public PartyDetails()
        {

        }

        public PartyDetails(int v1, int v2, string v3, int v4, string v5, int v6, int v7, double v8, double v9, string v10, string v11, int v12, int v13, int v14, int v15, string v16)
        {
            //this.v1 = v1;
            this.userId = v2.ToString();
            this.title = v3;
            this.picture = "p" + v4 + ".jpg";
            this.description = v5;
            this.type = v6;
            this.price = v7;
            this.latt = v8;
            this.lon = v9;
            this.when = DateTime.Parse(v10);
            this.where = v11;
            this.ageMin = v12;
            this.maxParticipants = v13;
            this.rating = v14;
            this.partyId = Guid.NewGuid().ToString();
            this.location = new Point(latt, lon);
            //this.v15 = v15;
            //this.v16 = v16;
        }

        [JsonProperty("Id")]
        public string Id { get; set; }
        //public int id { get; set; }
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

        [JsonProperty("partyId")]
        public string partyId { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("picture")]
        public string picture { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("type")]
        public int type { get; set; }

        [JsonProperty("price")]
        public int price { get; set; }

        [JsonProperty("ageMin")]
        public int ageMin { get; set; }

        [JsonProperty("ageMax")]
        public int ageMax { get; set; }

        [JsonProperty("latt")]
        public double latt { get; set; }

        [JsonProperty("lon")]
        public double lon { get; set; }

        [JsonProperty("location")]
        public Point location;

        [JsonProperty("when")]
        public DateTime when { get; set; }

        [JsonProperty("where")]
        public string where { get; set; }

        [JsonProperty("rating")]
        public int rating { get; set; }

        [JsonProperty("maxparticipants")]
        public int maxParticipants { get; set; }

        //public int restrictionAge { get; set; }
        //public int restrictionParticipants { get; set; }
        //public float restrictionRating { get; set; }
        //public float restrictionPrice { get; set; }
        //public DateTime restrictionTime { get; set; }

    }
}
