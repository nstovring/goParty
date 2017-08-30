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
	public partial class AtendeeView : ContentView
	{
		public AtendeeView ()
		{
			InitializeComponent ();
            BindingContext = new AttendeeViewModel();
		}

        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            Application.Current.MainPage.DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            Application.Current.MainPage.DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
    }
}