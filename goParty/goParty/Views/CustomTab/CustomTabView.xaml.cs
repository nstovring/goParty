using goParty.Abstractions;
using goParty.Helpers;
using goParty.Pages;
using goParty.ViewModels;
using goParty.Views.CustomFilterView;
using goParty.Views.CustomTab;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
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

        public void OnFilterImageTapped(object sender, EventArgs e)
        {
            AnimateRow();
            filterOpen = !filterOpen;
        }
        private Animation _animation;
        private double _initialHeight = 150;

        private void AnimateRow()
        {
            //if (!filterOpen)
            //{
            //    // Move back to original height
            //
            //     var animation = new Animation(v => xamlFilterView.HeightRequest = v, 0, _initialHeight);
            //     animation.Commit(this, "SimpleAnimation", 16, 500, Easing.Linear, (v, c) => xamlFilterView.HeightRequest = _initialHeight, () => false);
            //
            //
            //    _animation = new Animation(
            //        (d) => filterRow.Height = new GridLength(Clamp(d, 0, double.MaxValue)),
            //        filterRow.Height.Value, _initialHeight, Easing.Linear, () => _animation = null);
            //}
            //else
            //{
            //    var animation = new Animation(v => xamlFilterView.HeightRequest = v, _initialHeight, 0);
            //    animation.Commit(this, "SimpleAnimation", 16, 500, Easing.Linear, (v, c) => xamlFilterView.HeightRequest = 0, () => false);
            //
            //    // Hide the row
            //    _animation = new Animation(
            //        (d) => filterRow.Height = new GridLength(Clamp(d, 0, double.MaxValue)),
            //        _initialHeight, 0, Easing.Linear, () => _animation = null);
            //}
            //
            //_animation.Commit(this, "the animation");
        }

        private double Clamp(double value, double minValue, double maxValue)
        {
            if (value < minValue)
            {
                return minValue;
            }

            if (value > maxValue)
            {
                return maxValue;
            }

            return value;
        }


        private void Refresh()
        {
            InitializeView();
        }

        public CustomTabView()
        {
            InitializeComponent();
            searchBar.SearchButtonPressed += SearchBar_SearchButtonPressed;
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string searchString = searchBar.Text;
            Geocoder geocoder = new Geocoder();
            var points = await geocoder.GetPositionsForAddressAsync(searchString);
            if (points.Count() < 1)
            {
                searchBar.Text = null;
                return;
            }
            FilterSettings.SearchPosition = points.ToList()[0];
            FilterSettings.FilterChanged?.Invoke(this);
            await ItemSelected?.Invoke(this, selectedItem);
        }

        bool filterOpen = false;
      

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

            //GoToSelectedItem(headerItems[0], 0);
            //headerItems[0].OnTapped(this, 0);
        }

        public ObservableRangeCollection<HeaderItem> headerItems = new ObservableRangeCollection<HeaderItem>();


        public delegate Task OnSearch(object sender, Position position);
        public OnSearch Searched;

        public delegate Task OnItemSelected(object sender, int index);
        public OnItemSelected ItemSelected;

        public async void GoToSelectedItem(object sender, int index)
        {
            foreach (var item in headerItems)
            {
                item.OnUnselected(this, index);
            }
            selectedItem = index;
            ContentView senderView = (ContentView)AbsLayout.Children[index];
            double senderTranslation = senderView.TranslationX;


            TaskCompletionSource<bool> tsk = new TaskCompletionSource<bool>();

            foreach (var item in AbsLayout.Children)
            {
                double itemTranslation = item.TranslationX;
                item.TranslateTo(itemTranslation - senderTranslation, 0, 250, Easing.Linear);
            }
            tsk.SetResult(true);
            //AbsLayout.TranslateTo(-(selectedItem * App.ScreenWidth), 0, 250, Easing.Linear);
            await tsk.Task;
            await ItemSelected?.Invoke(this, index);
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