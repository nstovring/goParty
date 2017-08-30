using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Services;
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
	public partial class UserCardView : ContentView, INotifyPropertyChanged
	{
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
            Initialize();
          
        }

        async void Initialize()
        {
            //Check if image has aleardy been loaded
            ImageHelper.ImageHelperItem item = ImageHelper.LoadedImages.FirstOrDefault(x => x.imageId == Attendee.partyPicture);
            ImageSource img = item?.image;
            PartyImageSource = img ?? await AzureStorage.LoadImage(Attendee.partyPicture);
        }

        private async void TapGesture_Tapped(object sender, EventArgs e)
        {
            await this.TranslateTo(this.Width, this.Y, animLength, Easing.SpringOut);
            await this.TranslateTo(0, this.Y -Height, animLength, Easing.SpringOut);
        }


        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
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
            await Content.TranslateTo(0, 0, animLength, Easing.SpringOut);
            Content.BackgroundColor = Color.White;
            cardDistance = (float)X;
            //this.RotateTo(0, AnimLength, Easing.SpringOut);
        }

        float modifier = 1.2f;
        private async void HandleTouch(float totalX)
        {
            await Content.TranslateTo(totalX * modifier, 0, animLength, Easing.SpringOut);
            cardDistance = totalX;
            Content.BackgroundColor = new Color(0, cardDistance/ App.ScreenWidth, 0);
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



        //override void OnPropertyChanged(string propName)
        //{
        //    if (PropertyChanged == null)
        //        return;
        //    PropertyChanged(this, new PropertyChangedEventArgs(propName));
        //}
    }
}