using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Spatial;
using goParty.Abstractions;
using goParty.Helpers;
using System.ComponentModel;

namespace goParty.Models
{
    public class PartyDetails : TableData
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
            //this.location = new Point(latt, lon);
            //this.v15 = v15;
            //this.v16 = v16;
        }

        public string documentDBId { get; set; }

        public string userId { get; set; }

        public string partyId { get; set; }

        public string title { get; set; }

        public string picture { get; set; }

        public string description { get; set; }

        public int type { get; set; }

        public int price { get; set; }

        public int ageMin { get; set; }

        public int ageMax { get; set; }

        public double latt { get; set; }

        public double lon { get; set; }

        public DateTime when { get; set; }

        public string where { get; set; }

        public int rating { get; set; }

        public int maxParticipants { get; set; }

        public string hostpicture { get; set; }
    }
}
