using goParty.Helpers;
using goParty.Models;
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
	public partial class CardListView : ContentView
	{
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.ICollection), typeof(CardListView), null,
            propertyChanged: OnItemsSourcePropertyChanged);

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CardListView)bindable).Refresh();
        }

        public ObservableRangeCollection<AttendeeListItem> ItemsSource
        {
            get
            {
                return (ObservableRangeCollection<AttendeeListItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public CardListView ()
		{
			InitializeComponent ();
        }

        private void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            Console.WriteLine("Panning!");
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
            ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            if (ItemsSource == null || ItemsSource.Count <= 0)
                return;
            
            PopulateView();
        }

        public void PopulateView()
        {
            ListStackLayout.Children.Clear();
            foreach (var item in ItemsSource)
            {
                UserCardView cardViewItem = new UserCardView(item);
                ListStackLayout.Children.Add(cardViewItem);
            }
            
        }
    }
}