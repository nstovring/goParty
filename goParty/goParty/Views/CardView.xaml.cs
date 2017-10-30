using goParty.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using goParty.Helpers;

namespace goParty.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CardView : ContentView, INotifyPropertyChanged
    {
        public int index;

        public ImageSource _icon;
        public ImageSource Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value, "Icon"); }
        }
        public ImageSource _backgroundImage;
        public ImageSource BackgroundImage
        {
            get { return _backgroundImage; }
            set { SetProperty(ref _backgroundImage, value, "BackgroundImage"); }
        }

        public string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value, "Title"); }
        }

        public string _subtitle;
        public string subTitle
        {
            get { return _subtitle; }
            set { SetProperty(ref _subtitle, value, "subTitle"); }
        }

        public string _leftDetail;
        public string LeftDetail
        {
            get { return _leftDetail; }
            set { SetProperty(ref _leftDetail, value, "LeftDetail"); }
        }

        public string _rightDetail;
        public string RightDetail
        {
            get { return _rightDetail; }
            set { SetProperty(ref _rightDetail, value, "RightDetail"); }
        }

        private CardListItem item;
        private ScrollView parent;
        public CardView (CardListItem item, ScrollView parent)
		{
            this.parent = parent;
            this.item = item;
            //Populate card with values
            if(item.attendeeListItem != null)
            {
                InitializeAttendeeCard(item.attendeeListItem);
            }
            else
            {
                InitializePartyCard(item.partyDetailsItem);
            }

            BindingContext = this;
			InitializeComponent ();
		}


        private void InitializePartyCard(PartyDetailsItem partyDetailsItem)
        {
            Icon = partyDetailsItem.hostpicture;
            BackgroundImage = partyDetailsItem.pictureImageSource;
            Title = partyDetailsItem.title;
            subTitle = partyDetailsItem.when.ToString();
            LeftDetail =  LocationHelper.distance(partyDetailsItem.latt,partyDetailsItem.lon,App.CurrentPosition.Latitude,App.CurrentPosition.Longitude,'K').ToString();
            RightDetail = "DKK" + partyDetailsItem.price;
        }

        private void InitializeAttendeeCard(AttendeeListItem attendeeListItem)
        {
            //Icon = //partyDetailsItem.hostpicture;
            BackgroundImage = attendeeListItem.picture;
            Title = attendeeListItem.name;
            subTitle = attendeeListItem.rating.ToString();
            //LeftDetail = "Current Location From me";
            //RightDetail = "DKK" + partyDetailsItem.price;
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

        bool selected = false;
        public async void TapGesture_Tapped(object sender, TappedEventArgs e)
        {
            //Fade to Details
            if (item.partyDetailsItem != null) {
                PartyCardView.instance.PartyDetailsItem = item.partyDetailsItem;
                PartyCardView.instance.TransitionTo(this.Y - DescriptionBox.Height, parent.ScrollY);
                //LargeImage.TranslateTo(0, -App.ScreenHeight, 500, Easing.Linear);
            }
            else
            {
                throw new NotImplementedException();
            }
            //OnTapped(sender, e);
            //selected = !selected;
        }

        // A delegate type for hooking up change notifications.
        public delegate void SwipedEventHandler(object sender, bool e);
        public event SwipedEventHandler Swiped;

        // A delegate type for hooking up change notifications.
        public delegate void TappedEventHandler(object sender, TappedEventArgs e);
        public event TappedEventHandler Tapped;

        protected virtual void OnTapped(object sender, TappedEventArgs e)
        {
            if (Tapped != null)
                Tapped(sender, e);
        }


        protected virtual void OnSwiped(bool e)
        {
            if (Swiped != null)
                Swiped(this, e);
        }


        public void OnSwipeLeft()
        {
            OnSwiped(false);
            //Declined Attendee
        }

        public void OnSwipeRight()
        {
            OnSwiped(true);
            //Accepted Attendee
        }
    }
}