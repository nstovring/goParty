using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Models.APIModels
{
    public class StripeTempToken
    {
        public StripeTempToken()
        {
        }
        public StripeTempToken(Token token)
        {
            Id = token.Id;
            Created = token.Created;
            LiveMode = token.LiveMode;
            Used = token.Used;
        }
        [JsonProperty("Created")]
        public DateTime Created { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("LiveMode")]
        public bool LiveMode { get; set; }
        [JsonProperty("Used")]
        public bool Used { get; set; }

        //public StripeCard Card { get; }
    }
}
