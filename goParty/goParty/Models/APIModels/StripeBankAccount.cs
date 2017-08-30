using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Models.APIModels
{
    public class StripeBankAccount
    {
        public string customerId { get; set; }
        public string account_number { get; set;}
        public string country { get; set; }
        public string currency { get; set; }
        public string account_holder_name { get; set; }

    }
}
