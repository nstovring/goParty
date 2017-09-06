using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using goParty.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using goParty.ViewModels;

namespace goParty.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManagementPage : ContentPage
	{
		public ManagementPage ()
		{
            InitializeComponent();
            ManagementPageViewModel viewModel = new ManagementPageViewModel(ManagementRelativeLayout);
            BindingContext = viewModel;

		}
	}
}