using goParty.Abstractions;
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
        public bool accepted { get; set; }
    }
}
