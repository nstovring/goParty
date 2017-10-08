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
	public partial class TabbedContentPage : ContentPage
	{
		public TabbedContentPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new TabbedContentPageViewModel();
            InitializeComponent();
            ((TabbedContentPageViewModel)BindingContext).TabView = TabView;
        }
	}
}