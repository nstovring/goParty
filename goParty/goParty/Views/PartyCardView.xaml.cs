using goParty.Models;
using goParty.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageCircle.Forms.Plugin.Abstractions;
namespace goParty.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartyCardView : ContentView
    {
        public double opacity = 0;
        public double prevY = 0;
        public double newY = 0;

        public static PartyCardView instance;

        public double _partyImageHeight = 200;
        public double PartyImageHeight
        {
            get { return _partyImageHeight; }
            set { SetProperty(ref _partyImageHeight, value, "PartyImageHeight"); }
        }
        public PartyDetailsItem _partyDetailsItem;

        public PartyDetailsItem PartyDetailsItem
        {
            get { return _partyDetailsItem; }
            set { SetProperty(ref _partyDetailsItem, value, "PartyDetailsItem"); }
        }

        public ImageSource _partydetailsImage;

        public ImageSource PartyDetailsImage
        {
            get { return _partydetailsImage; }
            set { SetProperty(ref _partydetailsImage, value, "PartyDetailsImage"); }
        }

        Image AnimateableImage;
        View imageInFrame;
        public PartyCardView()
        {
            try
            {
                instance = this;
                BindingContext = this;
                InitializeComponent();

                CreateCard();

                prevY = App.ScreenHeight;
                if (PartyDetailsItem == null)
                    TransitionFrom();
            }catch(Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error!", "Error Displaying Party Page" + ex.Message, "OK");
                throw new Exception(ex.Message);
            }
        }

        private void PartyScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            scrollY = e.ScrollY;
        }

        double scrollY = 0;

        uint animationSpeed = 500;


        void CreateCard()
        {
            if (imageInFrame == null)
            {
                AnimateableImage = new Image
                {
                    Aspect = Aspect.AspectFill
                };

                Frame frame = new Frame()
                {
                    Padding = 0,
                    Margin = 0,
                    Content = AnimateableImage
                };

                //imageInFrame.Content 
                imageInFrame = new ContentView()
                {
                    Padding = 0,
                    Margin = 0,
                    Content = frame
                };
                AbsLayout.Children.Add(imageInFrame, new Rectangle(0, App.ScreenHeight, App.ScreenWidth, _partyImageHeight));
                AbsoluteLayout.SetLayoutFlags(imageInFrame, AbsoluteLayoutFlags.None);
            }
            if(PartyDetailsItem != null)
            {
                AnimateableImage.Source = PartyDetailsItem.pictureImageSource;
            }

        }

        public async void TransitionTo(double startY, double scrollAmount)
        {
            CreateCard();

            AbsLayout.LowerChild(imageInFrame);

            scrollY = scrollAmount;
            //double scrollX = PartyScrollView.ScrollX;
            startY = startY - scrollY;
            prevY = startY;
            //imageInFrame..Source = PartyDetailsItem.pictureImageSource;
            imageInFrame.TranslationY = startY + _partyImageHeight / 2 - App.ScreenHeight;
            //AnimateableImage.
            DetailsScrollView.FadeTo(1, animationSpeed * 2, Easing.CubicInOut);
            HeaderLayout.FadeTo(1, animationSpeed * 2, Easing.CubicInOut);
            //AnimateableImage.ScaleTo(2, 250, Easing.Linear);
            //imageInFrame.HeightRequest = PartyImageHeight;
            imageInFrame.FadeTo(1, animationSpeed, Easing.CubicInOut);
            await imageInFrame.TranslateTo(imageInFrame.X, -App.ScreenHeight, animationSpeed, Easing.CubicInOut);
            PartyDetailsImage = PartyDetailsItem.pictureImageSource;
            //await imageInFrame.FadeTo(0, animationSpeed, Easing.CubicInOut);

            InputTransparent = false;
        }

        public async void ClickedEventHandler(object sender, EventArgs e)
        {
            await PartyDetailsItem.ExecuteJoinPartyCommand();
        }

        public async void TransitionFrom()
        {
            DetailsScrollView.FadeTo(0, animationSpeed, Easing.CubicInOut);
            HeaderLayout.FadeTo(0, animationSpeed, Easing.CubicInOut);
            //AbsLayout.Children.Remove(imageInFrame);
            //AbsLayout.Children.Insert(0, imageInFrame);
            if (imageInFrame != null)
            {
                //await imageInFrame.FadeTo(1, animationSpeed, Easing.CubicInOut);
                PartyDetailsImage = null;
                await imageInFrame.TranslateTo(imageInFrame.X, -App.ScreenHeight + prevY + _partyImageHeight/2 + DetailsScrollView.ScrollY, animationSpeed, Easing.CubicInOut);
                await imageInFrame.FadeTo(0, animationSpeed, Easing.CubicInOut);
            }
            InputTransparent = true;
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
    }
}