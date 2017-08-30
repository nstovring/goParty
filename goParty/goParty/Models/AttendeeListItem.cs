using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models.APIModels;
using goParty.Services;
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

        public bool Paid
        {
            get { return _paid; }
            set { SetProperty(ref _paid, value, "Paid"); }
        }

        public bool Accepted
        {
            get { return _accepted; }
            set { SetProperty(ref _accepted, value, "Accepted", AcceptAttendeeCommand); }
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

        Action acceptAttendeecmd;
        public Action AcceptAttendeeCommand => acceptAttendeecmd ?? (acceptAttendeecmd = new Action(async () => await ExecuteAcceptAttendeeCommand().ConfigureAwait(false)));


        private async Task ExecuteAcceptAttendeeCommand()
        {
            //Charge Customer
            var stripeService = ServiceLocator.Instance.Resolve<IStripeProvider>();
            string id;
            var loginProvider = DependencyService.Get<ILoginProvider>();
            Account acc = loginProvider.RetreiveAccountFromSecureStore();
            acc.Properties.TryGetValue(Constants.stripeAccountIdPropertyName, out id);

            string JParty = JsonConvert.SerializeObject(partyDetails);

            StripeCharge stripeCharge = new StripeCharge()
            {
                amount = price,
                customerId = customerid,
                currency = "dkk",
                receiverId = id, //This users id
                partyId = partyid,
                ObjectJson = JParty
            };
            StripeResponse response = await stripeService.ChargeCustomer(stripeCharge);

            //Update Table
            //cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            var Table = cloudService.GetTable<AttendeeDetails>();
            AttendeeDetails attendee = new AttendeeDetails()
            {
                Id = attendeeID,
                userId = this.userId,
                partyId = this.partyid,
                UpdatedAt = DateTime.Now,
                accepted = true
            };
            await Table.UpdateItemAsync(attendee);
        }

        Command declineAttendeecmd;
        public Command DeclineAttendeeCommand => declineAttendeecmd ?? (declineAttendeecmd = new Command(async () => await ExecuteDeclineAttendeeCommand().ConfigureAwait(false)));

        private Task ExecuteDeclineAttendeeCommand()
        {
            throw new NotImplementedException();
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
