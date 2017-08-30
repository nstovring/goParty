using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using goParty.Models.APIModels;
using goParty.Views;

namespace goParty.ViewModels
{
    public class AttendeeViewModel : BaseViewModel
    {

        public ObservableRangeCollection<AttendeeListItem> attendees = new ObservableRangeCollection<AttendeeListItem>();
        public ObservableRangeCollection<AttendeeListItem> Attendees
        {
            get { return attendees; }
            set { SetProperty(ref attendees, value, "Attendees"); }
        }

        public AttendeeViewModel()
        {
            Initialize();
        }

        public async void Initialize()
        {
            Attendees = new ObservableRangeCollection<AttendeeListItem>();
            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            var AttendeeTable = cloudService.GetTable<AttendeeDetails>();
            var Table = cloudService.GetTable<UserDetails>();
            List<UserDetails> temp = new List<UserDetails>();
            ICollection<AttendeeDetails> attendeeDetails = await AttendeeTable.ReadAllItemsAsync();


            if (attendeeDetails == null || attendeeDetails.Count <= 0)
                return;

            foreach (var att in attendeeDetails)
            {
                UserDetails tempDetails = await Table.ReadItemAsync(att.userId);
                tempDetails.Id = att.chargeId;
                tempDetails.partyID = att.partyId;
                tempDetails.attendeeID = att.Id;
                AttendeeListItem attitem = new AttendeeListItem(tempDetails);
                attitem.userDetails = tempDetails;
                Attendees.Add(attitem);
            }
        }
    }
}
