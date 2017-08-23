using goParty.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
            ProfilePageViewModel profilePageViewModel = new ProfilePageViewModel();
            profilePageViewModel.stackLayout = StackLayout;
            profilePageViewModel.stackLayoutHosting = StackLayoutHosting;
            BindingContext = profilePageViewModel;
		}
	}
}