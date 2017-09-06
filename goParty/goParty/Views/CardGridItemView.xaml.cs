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
	public partial class CardGridItemView : ContentView
	{
        public string Title { get; set; }

        public string IconSource { get; set; }

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //((CardGridView)bindable).Refresh();
        }

        public CardGridItemView(MasterPageItem item)
		{
            Title = item.Title;
            IconSource = item.IconSource;
            BindingContext = this;
            InitializeComponent();
		}
	}
}