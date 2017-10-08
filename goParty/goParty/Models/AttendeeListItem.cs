using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models.APIModels;
using goParty.Services;
using goParty.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace goParty.Models
{
    public class AttendeeListItem : UserDetails, INotifyPropertyChanged
    {
        public UserDetails userDetails;
        PartyDetails partyDetails;
        ICloudService cloudService;
        public AttendeeListItem(UserDetails userDetails)
        {
            this.userId = userDetails.userId;
            customerid = userDetails.userId;
            name = userDetails.name;
            rating = userDetails.rating;
            picture = userDetails.picture;
            partyID = userDetails.partyID;
            attendeeID = userDetails.attendeeID;
        }

        public async Task GetParty()
        {
            cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            var Table = cloudService.GetTable<PartyDetails>();
            partyDetails = await Table.ReadItemAsync(partyID);
            price = partyDetails.price;
            partyPicture = partyDetails.picture;
        }

        public int price;
        public string customerid;
        public string partyid;
        public string partyPicture;

        string _propTitle = string.Empty;
        bool _propIsBusy;
        public bool _paid = false;
        public bool _accepted = false;
        public bool _declined = false;

        public bool Paid
        {
            get { return _paid; }
            set { SetProperty(ref _paid, value, "Paid"); }
        }


        public AtendeeView.AttendeeType _attendeeType = AtendeeView.AttendeeType.Pending;

        public AtendeeView.AttendeeType AttendeeType
        {
            get { return _attendeeType; }
            set { SetProperty(ref _attendeeType, value, "AttendeeType", null); }
        }

        public bool Accepted
        {
            get { return _accepted; }
            set { SetProperty(ref _accepted, value, "Accepted");}
        }

        public bool Declined
        {
            get { return _declined; }
            set { SetProperty(ref _declined, value, "Declined"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get { return _propTitle; }
            set { SetProperty(ref _propTitle, value, "Title"); }
        }

        public bool IsBusy
        {
            get { return _propIsBusy; }
            set { SetProperty(ref _propIsBusy, value, "IsBusy"); }
        }

        Task acceptAttendeecmd;
        public Task AcceptAttendeeCommand => acceptAttendeecmd ?? (acceptAttendeecmd = new Task(async () => await ExecuteAcceptAttendeeCommand().ConfigureAwait(false)));


        public async Task ExecuteAcceptAttendeeCommand()
        {
            //Charge Customer
            //var stripeService = ServiceLocator.Instance.Resolve<IStripeProvider>();
            //string id;
            //var loginProvider = DependencyService.Get<ILoginProvider>();
            //Account acc = loginProvider.RetreiveAccountFromSecureStore();
            //acc.Properties.TryGetValue(Constants.stripeAccountIdPropertyName, out id);
            //
            //string JParty = JsonConvert.SerializeObject(partyDetails);
            //
            //StripeCharge stripeCharge = new StripeCharge()
            //{
            //    amount = price,
            //    customerId = customerid,
            //    currency = "dkk",
            //    receiverId = id, //This users id
            //    partyId = partyid,
            //    ObjectJson = JParty
            //};
            //StripeResponse response = await stripeService.ChargeCustomer(stripeCharge);

            //Update Table
            cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            var Table = cloudService.GetTable<AttendeeDetails>();
            AttendeeDetails attendee = new AttendeeDetails()
            {
                Id = attendeeID,
                userId = this.userId,
                partyId = this.partyID,
                UpdatedAt = DateTime.Now,
                accepted = true,
                declined = false
            };
            await Table.UpdateItemAsync(attendee);
            Accepted = true;
        }

        Task declineAttendeecmd;
        public Task DeclineAttendeeCommand => declineAttendeecmd ?? (declineAttendeecmd = new Task(async () => await ExecuteDeclineAttendeeCommand().ConfigureAwait(false)));

        public async Task ExecuteDeclineAttendeeCommand()
        {
            if (Declined == false)
                return;
            var Table = cloudService.GetTable<AttendeeDetails>();
            AttendeeDetails attendee = new AttendeeDetails()
            {
                Id = attendeeID,
                userId = this.userId,
                partyId = this.partyid,
                UpdatedAt = DateTime.Now,
                accepted = false,
                declined = true
            };
            await Table.UpdateItemAsync(attendee);
            Declined = true;
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        
        protected void SetProperty<T>(ref T store, T value, string propName, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(store, value))
                return;
            store = value;
            if (onChanged != null)
                onChanged();
            OnPropertyChanged(propName);
        }

        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
