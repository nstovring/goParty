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
using goParty.Pages;

namespace goParty.ViewModels
{
    public class AttendeeViewModel : BaseViewModel, IRefreshable
    {
        public ObservableRangeCollection<AttendeeListItem> attendees = new ObservableRangeCollection<AttendeeListItem>();
        public ObservableRangeCollection<AttendeeListItem> Attendees
        {
            get { return attendees; }
            set { SetProperty(ref attendees, value, "Attendees"); }
        }

        public AttendeeViewModel(AtendeeView parent)
        {
            this.parent = parent;
            Initialize(parent.attendeeType);
        }

        //public override void ExecuteBackButtonClickedCommand()
        //{
        //    TabbedMainPage.Instance.InputTransparent = false;
        //    base.ExecuteBackButtonClickedCommand();
        //}

        public async void Initialize(AtendeeView.AttendeeType attendeeType)
        {
            Attendees = new ObservableRangeCollection<AttendeeListItem>();
            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            var AttendeeTable = cloudService.GetTable<AttendeeDetails>();
            var Table = cloudService.GetTable<UserDetails>();
            List<AttendeeListItem> temp = new List<AttendeeListItem>();
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
                attitem.Accepted = att.accepted;
                attitem.Declined = !att.accepted;
                attitem.Paid = att.paid;
                attitem.userDetails = tempDetails;

                switch (attendeeType)
                {
                    case AtendeeView.AttendeeType.Pending:
                        if(!attitem.Accepted && !attitem.Declined)
                            temp.Add(attitem);
                        break;
                    case AtendeeView.AttendeeType.Accepted:
                        if (attitem.Accepted)
                            temp.Add(attitem);
                        break;
                    case AtendeeView.AttendeeType.Declined:
                        if (attitem.Declined)
                            temp.Add(attitem);
                        break;
                }

            }
            ImageHelper.LoadedImages.Clear();
            Attendees.AddRange(temp);
        }

        public Task Refresh()
        {
            throw new NotImplementedException();
        }

        //ObservableRangeCollection<AttendeeListItem> GetAttendees(this.parent.)
        //{
        //    return null;
        //}
    }
}
