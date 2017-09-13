using goParty.Abstractions;
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
	public partial class AtendeeView : ContentView , IRefreshable
	{
        public AttendeeViewModel viewModel;

        public static readonly BindableProperty AttendeeTypeProperty =
           BindableProperty.Create(nameof(attendeeType), typeof(AttendeeType), typeof(AtendeeView), AttendeeType.Pending,
           propertyChanged: OnItemsSourcePropertyChanged);

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //((CardListView)bindable).Refresh();
        }

        public enum AttendeeType {Accepted, Declined, Pending };

        public AttendeeType attendeeType{
            get
            {
                return (AttendeeType)GetValue(AttendeeTypeProperty);
            }
            set
            {
                SetValue(AttendeeTypeProperty, value);
            }
        }

        public AtendeeView ()
		{
			InitializeComponent ();
            viewModel = new AttendeeViewModel(this);
            BindingContext = viewModel;
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

        public Task Refresh()
        {
            throw new NotImplementedException();
        }
    }
}