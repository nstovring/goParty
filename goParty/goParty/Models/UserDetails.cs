using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Models
{
    public class UserDetails : TableData
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string description { get; set; }

        public int rating { get; set; }
        public int age { get; set; }
    }
}
