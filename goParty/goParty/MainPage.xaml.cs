using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Models;
using goParty.Helpers;

namespace goParty.Pages
{
	public partial class MainPage : ContentPage
	{

        public ListView ListView { get { return listView; } }


        public MainPage()
		{
			InitializeComponent();
            var masterPageItems = MasterDetailPageManager.PageItems();
            listView.ItemsSource = masterPageItems;

        }
	}
}
