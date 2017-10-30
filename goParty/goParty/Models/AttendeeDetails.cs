using goParty.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Models
{
    class AttendeeDetails : TableData
    {
        public string userId { get; set; }
        public string partyId { get; set; }

        public bool paid { get; set; }
        [JsonProperty(PropertyName = "accepted")]
        public bool accepted { get; set; }
        public bool declined { get; set; }

        public string chargeId { get; set; }
    }
}
