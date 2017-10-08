using goParty.Models;
using goParty.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartyCardView : ContentView
    {
        public double opacity = 0;
        public double prevY = 0;
        public double newY = 0;

        public static PartyCardView instance;

        public double _partyImageHeight = 150;
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
        Image AnimateableImage;
        Frame imageInFrame;
        public PartyCardView()
        {
            instance = this;
            BindingContext = this;
            InitializeComponent();

            AnimateableImage = new Image
            {
                Aspect = Aspect.AspectFill
            };

            imageInFrame = new Frame()
            {
                Padding = 0,
                Margin = 0,
                Content = AnimateableImage
            };

            AbsLayout.Children.Add(imageInFrame, new Rectangle(0, App.ScreenHeight, App.ScreenWidth, PartyImageHeight));
            AbsoluteLayout.SetLayoutFlags(imageInFrame, AbsoluteLayoutFlags.None);
            prevY = App.ScreenHeight;

            CardListView.scrollView.Scrolled += PartyScrollView_Scrolled;

            if (PartyDetailsItem == null)
                TransitionFrom();
        }

        private void PartyScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            scrollY = e.ScrollY;
        }

        double scrollY = 0;

        uint animationSpeed = 500;


        public async void TransitionTo(double startY)
        {
            //double scrollX = PartyScrollView.ScrollX;
            startY = startY - scrollY;
            prevY = startY;
            AnimateableImage.Source = PartyDetailsItem.pictureImageSource;
            imageInFrame.TranslationY = -App.ScreenHeight + startY;
            //AnimateableImage.
            ContentGrid.FadeTo(1, animationSpeed, Easing.CubicInOut);
            HeaderLayout.FadeTo(1, animationSpeed, Easing.CubicInOut);
            //AnimateableImage.ScaleTo(2, 250, Easing.Linear);
            imageInFrame.HeightRequest = PartyImageHeight;
            await imageInFrame.FadeTo(1, animationSpeed, Easing.CubicInOut);
            await imageInFrame.TranslateTo(imageInFrame.X, -App.ScreenHeight, animationSpeed, Easing.CubicInOut);
            InputTransparent = false;
        }

        public async void TransitionFrom()
        {
            ContentGrid.FadeTo(0, animationSpeed, Easing.CubicInOut);
            HeaderLayout.FadeTo(0, animationSpeed, Easing.CubicInOut);
            if (AnimateableImage != null)
            {
                await imageInFrame.TranslateTo(imageInFrame.X, -App.ScreenHeight + prevY, animationSpeed, Easing.CubicInOut);
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