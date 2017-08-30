using goParty.Abstractions;
using goParty.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace goParty.ViewModels
{
    public class UserCardViewModel : BaseViewModel
    {
        public AttendeeListItem attendee;
        public AttendeeListItem Attendee
        {
            get { return attendee; }
            set { SetProperty(ref attendee, value, "Attendee"); }
        }

        public UserCardViewModel(AttendeeListItem item)
        {
            attendee = item;
        }
    }
}
