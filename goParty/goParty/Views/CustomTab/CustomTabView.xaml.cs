using goParty.Abstractions;
using goParty.Helpers;
using goParty.Pages;
using goParty.Views.CustomTab;
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
	public partial class CustomTabView : ContentView
    {
        public int selectedItem = 0;
        double animationSlideRange = App.ScreenWidth / 4f;
        double distanceX = 0;
        public ObservableRangeCollection<ContentView> ItemsSource
        {
            get
            {
                return (ObservableRangeCollection<ContentView>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.ICollection), typeof(CustomTabView), null,
            propertyChanged: OnItemsSourcePropertyChanged);

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomTabView)bindable).Refresh();
        }


        public void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            RootMasterDetailPage.Instance.IsPresented = !RootMasterDetailPage.Instance.IsPresented;
        }
        private void Refresh()
        {
            InitializeView();
        }

        public CustomTabView ()
		{
            InitializeComponent();
        }

        public void InitializeView()
        {
            OffsetViews();
        }

        void OffsetViews()
        {
            double offset = 0;
            int index = 0;
            foreach (var item in ItemsSource)
            {
                //Add View To Header
                CardListView cardListView = (CardListView)item;
                //BaseViewModel vm = item.BindingContext as BaseViewModel;
                HeaderItem headerItem = new HeaderItem(cardListView.Title, index);
                
                //Add go to specific view handler
                headerItem.Tapped += GoToSelectedItem;
                HeaderStackLayout.Children.Add(headerItem);
                headerItems.Add(headerItem);
                //Add View to Absolute layout
                AbsLayout.Children.Add(item);
                item.TranslationX = offset;
                offset += App.ScreenWidth;
                index++;
            }
        }

        public ObservableRangeCollection<HeaderItem> headerItems = new ObservableRangeCollection<HeaderItem>();

        public delegate Task OnItemSelected(object sender, int index);
        public OnItemSelected ItemSelected;

        public void GoToSelectedItem(object sender, int index)
        {
            foreach (var item in headerItems)
            {
                item.OnUnselected(this, index);
            }
            selectedItem = index;
            ContentView senderView = (ContentView)AbsLayout.Children[index];
            double senderTranslation = senderView.TranslationX;
            foreach (var item in AbsLayout.Children)
            {
                double itemTranslation = item.TranslationX;
                item.TranslateTo(itemTranslation-senderTranslation, 0, 250, Easing.Linear);
            }
            //AbsLayout.TranslateTo(-(selectedItem * App.ScreenWidth), 0, 250, Easing.Linear);
            ItemSelected?.Invoke(this, index);
        }

        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            //if (expanded)
            //    return;

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

        private void HandleTouch(float totalX)
        {
            distanceX = totalX;
            AbsLayout.TranslationX += distanceX;
        }

        private void HandleTouchStart()
        {
            //throw new NotImplementedException();
        }

        private async void HandleTouchEnd()
        {

            if (AbsLayout.TranslationX > animationSlideRange)
            {
                //OnSwipeRight();
                return;
            }
            else if (AbsLayout.TranslationX < -(animationSlideRange))
            {
                //OnSwipeLeft();
                return;
            }

            
            //cardDistance = (float)X;
            //sliding = false;
        }
    }
}