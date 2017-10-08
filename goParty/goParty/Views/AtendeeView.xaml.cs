using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AtendeeView : ContentView , IRefreshable
	{
        public AttendeeViewModel viewModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableRangeCollection<AttendeeListItem> attendees = new ObservableRangeCollection<AttendeeListItem>();
        public ObservableRangeCollection<AttendeeListItem> Attendees
        {
            get { return attendees; }
            set { SetProperty(ref attendees, value, "Attendees"); }
        }

        public static readonly BindableProperty AttendeeTypeProperty =
           BindableProperty.Create(nameof(attendeeType), typeof(AttendeeType), typeof(AtendeeView), AttendeeType.Null,
           propertyChanged: OnItemsSourcePropertyChanged);

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

            ((AtendeeView)bindable).Initialize((AttendeeType)newValue);
            //((CardListView)bindable).Refresh();
        }

        public enum AttendeeType {Accepted, Declined, Pending, Null };

        public AttendeeType attendeeType{
            get
            {
                return (AttendeeType)GetValue(AttendeeTypeProperty);
            }
            set
            {
                SetValue(AttendeeTypeProperty, value);
            }
        }

        public AtendeeView ()
		{
            //viewModel = new AttendeeViewModel(this);
		}


        public async void Initialize(AttendeeType attendeeType)
        {

            await Refresh();
            BindingContext = this;
            InitializeComponent();
        }

        public async Task Refresh()
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
                try
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
                    switch (attendeeType)
                    {
                        case AttendeeType.Pending:
                            if (attitem.Accepted == false && attitem.Declined == false)
                                temp.Add(attitem);
                            break;
                        case AttendeeType.Accepted:
                            if (attitem.Accepted == true)
                                temp.Add(attitem);
                            break;
                        case AttendeeType.Declined:
                            if (attitem.Declined == true)
                                temp.Add(attitem);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", att.Id +": "+ ex.Message + " You have been alerted", "OK");
                    return;
                    //throw new NullReferenceException();
                }
            }
            ImageHelper.LoadedImages.Clear();
            Attendees.AddRange(temp);
        }


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