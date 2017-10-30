using Adapt.Presentation.Controls;
using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace goParty.Pages.ManagementPages.AttendeePages
{
    public class AttendeesViewModel : BaseViewModel, IRefreshable
    {
        public enum AttendeeViewType { All, Pending, Accepted, Denied };

        public AttendeeViewType myType = AttendeeViewType.All;


        public ObservableRangeCollection<AttendeeListItem> attendees;

        public ObservableRangeCollection<AttendeeListItem> Attendees
        {
            get { return attendees; }
            set { SetProperty(ref attendees, value, "Attendees"); }
        }

        public WrapLayout wrapLayout;

        public AttendeesViewModel(WrapLayout wrapLayout, AttendeeViewType myType)
        {
            try
            {
                this.myType = myType;
                this.wrapLayout = wrapLayout;
                Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task GetAttendees()
        {
            wrapLayout.Children.Clear();
            Attendees = new ObservableRangeCollection<AttendeeListItem>();
            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            var AttendeeTable = cloudService.GetTable<AttendeeDetails>();
            var Table = cloudService.GetTable<UserDetails>();
            List<AttendeeListItem> temp = new List<AttendeeListItem>();

            ICollection<AttendeeDetails> attendeeDetails = await AttendeeTable.GetTable().ToListAsync();
            //Only Get accepted attendees
            if (attendeeDetails == null)
                return;

            attendeeDetails = SortAttendeeDetails(attendeeDetails).ToList();
            foreach (var att in attendeeDetails)
            {

                UserDetails tempDetails = await Table.ReadItemAsync(att.userId);
                tempDetails.Id = att.chargeId;
                tempDetails.partyID = att.partyId;
                tempDetails.attendeeID = att.Id;
                AttendeeListItem attitem = new AttendeeListItem(tempDetails);
                attitem.Accepted = att.accepted;
                attitem.Declined = att.declined;
                attitem.Paid = att.paid;
                attitem.userDetails = tempDetails;
                wrapLayout.Children.Add(new AttendeeView(attitem));
            }

        }

        

        Command refreshCommand;
        public Command RefreshCommand => refreshCommand ?? (refreshCommand = new Command(async () => await Refresh().ConfigureAwait(false)));


        Command acceptAttendeecmd;
        public Command AcceptAttendeeCommand => acceptAttendeecmd ?? (acceptAttendeecmd = new Command(async () => await AcceptMarkedAttendees().ConfigureAwait(false)));

        Command declineAttendeecmd;
        public Command DeclineAttendeeCommand => declineAttendeecmd ?? (declineAttendeecmd = new Command(async () => await DeclineMarkedAttendees().ConfigureAwait(false)));

        public async Task AcceptMarkedAttendees()
        {
            List<View> attendeeList = wrapLayout.Children.Where(y => (((AttendeeView)y).marked == true)).ToList();
            attendeeList.ForEach((async x => await (((AttendeeView)x).AcceptAttendee())));
            await Refresh();
        }

        public async Task DeclineMarkedAttendees()
        {
            List<View> attendeeList = wrapLayout.Children.Where(y => (((AttendeeView)y).marked == true)).ToList();
            attendeeList.ForEach((async x => await (((AttendeeView)x).DeclineAttendee())));
            await Refresh();
        }

        ICollection<AttendeeDetails> SortAttendeeDetails(ICollection<AttendeeDetails> attendeeDetails)
        {
            switch (myType)
            {
                case AttendeeViewType.All:
                    return attendeeDetails;
                case AttendeeViewType.Accepted:
                    return attendeeDetails.Where(x => x.accepted == true).ToList();
                case AttendeeViewType.Denied:
                    return attendeeDetails.Where(x => x.declined == true).ToList();
                case AttendeeViewType.Pending:
                    return attendeeDetails.Where(x => x.accepted == false).ToList();
            }
            return null;
        }


        public async Task Refresh()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await GetAttendees();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        class AttendeeView : ContentView
        {
            public ImageSource attendeeImage;
            public AttendeeListItem attendee;

            public bool marked = false;

            public double ImageSize = App.ScreenWidth / 3.5;

            public AttendeeView(AttendeeListItem valueSource)
            {
                attendee = valueSource;
                attendeeImage = valueSource.picture;
                BindingContext = this;
                Content = new CircleImage()
                {
                    HeightRequest = ImageSize,
                    Source = attendeeImage,
                    WidthRequest = ImageSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                var gesture = new TapGestureRecognizer()
                {
                    Command = new Command(async () => await ExecuteTappedCommand().ConfigureAwait(false))
                };

                this.GestureRecognizers.Add(gesture);
            }

            public async Task AcceptAttendee()
            {
                await attendee.ExecuteAcceptAttendeeCommand();
            }

            public async Task DeclineAttendee()
            {
                await attendee.ExecuteDeclineAttendeeCommand();
            }

            async Task ExecuteTappedCommand()
            {
                if (!marked)
                {
                    await this.ScaleTo(0.75, 250, Easing.CubicOut);
                }
                else
                {
                    await this.ScaleTo(1, 250, Easing.CubicOut);
                }
                marked = !marked;
            }
        }
    }
}
