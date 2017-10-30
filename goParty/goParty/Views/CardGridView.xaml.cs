using goParty.Helpers;
using goParty.Models;
using goParty.Views;
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
	public partial class CardGridView : ContentView
	{
        int collumns = 2;
        int rows = 0;
        public static readonly BindableProperty ItemsSourceProperty =
           BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.ICollection), typeof(CardGridView), null,
           propertyChanged: OnItemsSourcePropertyChanged);

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //((CardGridView)bindable).Refresh();
        }
        public ObservableRangeCollection<MasterPageItem> ItemsSource
        {
            get
            {
                return (ObservableRangeCollection<MasterPageItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        public CardGridView ()
		{
            List<CardGridItemView> list = new List<CardGridItemView>();
            ItemsSource = MasterDetailPageManager.PageItems();
            foreach (var item in ItemsSource)
            {
                CardGridItemView card = new CardGridItemView(item);
                
                list.Add(card);
            }
            rows = ItemsSource.Count / collumns;
            var grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            }

            for (int i = 0; i < collumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < collumns; j++)
                {
                    grid.Children.Add(list[i*rows + j], i, j);
                }
            }
            Content = grid;
		}

        public void OnSelected()
        {

        }

        public delegate void ItemSelectedEventHandler(object sender, SelectedItemChangedEventArgs e);

        public CardGridItemView SelectedItem;
    }
}