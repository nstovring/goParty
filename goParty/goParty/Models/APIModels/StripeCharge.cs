using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Models.APIModels
{
    public class StripeCharge
    {
        public int amount { get; set; }
        public string currency { get; set; }
        public int application_fee { get; set; }
        public string customerId { get; set; }
        public string description { get; set; }
        public string statement_descriptor { get; set; }
        public string receiverId { get; set; }
        public string partyId { get; set; }
        public string ObjectJson { get; set; }

    }
}
