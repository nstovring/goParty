using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Services;
using goParty.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserCardView : ContentView, INotifyPropertyChanged
	{
        public int index = 0;
        float modifier = 1.2f;
        bool sliding = false;
        bool expanded = false;

        public AttendeeListItem attendee;
        public AttendeeListItem Attendee
        {
            get { return attendee; }
            set { SetProperty(ref attendee, value, "Attendee"); }
        }

        ImageSource partyImageSource;
        public ImageSource PartyImageSource
        {
            get { return partyImageSource; }
            set { SetProperty(ref partyImageSource, value, "PartyImageSource"); }
        }
        public float cardDistance = 0;
        const int animLength = 250;
        public UserCardView (AttendeeListItem item)
		{
			InitializeComponent ();
            Attendee = item;
            BindingContext = this;// new UserCardViewModel(item);
        }

        async void LoadPartyImage()
        {
            //Check if image has aleardy been loaded
            ImageHelper.ImageHelperItem item = ImageHelper.LoadedImages.FirstOrDefault(x => x.imageId == Attendee.partyPicture);
            ImageSource img = item?.image;
            PartyImageSource = img ?? await AzureStorage.LoadImage(Attendee.partyPicture);
        }

        private void TapGesture_Tapped(object sender, float e)
        {
            if (sliding)
                return;

            float requestedHeight = 0;
            if (expanded)
            {
                var animation = new Animation(v => this.HeightRequest = v, 400, 150);
                animation.Commit(this, "SimpleAnimation", 16, animLength, Easing.Linear);
                expanded = !expanded;
                requestedHeight = 150;
            }
            else
            {
                var animation = new Animation(v => this.HeightRequest = v, 150, 400);
                animation.Commit(this, "SimpleAnimation", 16, animLength, Easing.Linear);
                expanded = !expanded;
                requestedHeight = 400;
            }
            OnTapped(requestedHeight);
        }

        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (expanded)
                return;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    HandleTouchStart();
                    break;
                case GestureStatus.Running:
                    HandleTouch((float)e.TotalX);
                    break;
                case GestureStatus.Completed:
                    HandleTouchEnd();
                    break;
            }
        }

        private async void HandleTouchEnd()
        {
            await CardFrame.TranslateTo(0, 0, animLength, Easing.SpringOut);
            BackGrid.Opacity = 0;
            cardDistance = (float)X;
            sliding = false;
        }

        private async void HandleTouch(float totalX)
        {
            sliding = true;

            await CardFrame.TranslateTo(totalX * modifier, 0, animLength, Easing.SpringOut);
            cardDistance = totalX;
            BackGrid.Opacity = 1f-(Math.Abs(cardDistance)/ App.ScreenWidth);

            if(cardDistance > App.ScreenWidth/4f)
            {
                OnSwipeRight();
                return;
            }
            else if(cardDistance < -(App.ScreenWidth / 4f))
            {
                OnSwipeLeft();
                return;
            }
        }

        // to hande when a touch event begins
        public void HandleTouchStart()
        {
            cardDistance = 0;
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

        // A delegate type for hooking up change notifications.
        public delegate void SwipedEventHandler(object sender, bool e);
        public event SwipedEventHandler Swiped;

        // A delegate type for hooking up change notifications.
        public delegate void TappedEventHandler(object sender, float e);
        public event TappedEventHandler Tapped;

        protected virtual void OnTapped(float e)
        {
            if (Tapped != null)
                Tapped(this, e);
        }


        protected virtual void OnSwiped(bool e)
        {
            sliding = true;
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

        //Command RejectAttendeecmd;
        //public Command SaveCardDetailsCommand => saveCardDetailscmd ?? (saveCardDetailscmd = new Command(async () => await ExecuteSaveCardDetailsCommand().ConfigureAwait(false)));
        //
        //Command AcceptAttendeecmd;
        //public Command GetCardDetailsCommand => getCardDetailscmd ?? (getCardDetailscmd = new Command(async () => await ExecuteGetCardDetailsCommand().ConfigureAwait(false)));

    }
}