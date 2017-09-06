using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Models;
using goParty.Helpers;
using goParty.Views;

namespace goParty.Pages
{
	public partial class MainPage : ContentPage
	{
        public ListView ListView { get { return listView; } }
        public ObservableRangeCollection<MasterPageItem> ItemsSource;
        public MainPage()
		{
            ItemsSource = MasterDetailPageManager.PageItems();
            InitializeComponent();
            listView.ItemsSource = ItemsSource;
        }
    }
}
