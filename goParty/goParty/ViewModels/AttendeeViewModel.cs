using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using System;
using System.Threading.Tasks;
using goParty.Views;

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

        public AttendeeViewModel(AtendeeView parent, ObservableRangeCollection<AttendeeListItem> attendees)
        {
            this.parent = parent;

            Attendees = attendees; 

            //Initialize(parent.attendeeType);
        }

        //public override void ExecuteBackButtonClickedCommand()
        //{
        //    TabbedMainPage.Instance.InputTransparent = false;
        //    base.ExecuteBackButtonClickedCommand();
        //}

        

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
