using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Spatial;
using goParty.Abstractions;
namespace goParty.Models
{
    public class PartyDetailsDB : PartyDetails
    {
        public PartyDetailsDB()
        {

        }

        public PartyDetailsDB(int v1, int v2, string v3, int v4, string v5, int v6, int v7, double v8, double v9, string v10, string v11, int v12, int v13, int v14, int v15, string v16)
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
        public Point location;
    }
}
