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
	public partial class TabbedMainPage : TabbedPage
	{
        public static TabbedMainPage Instance;
		public TabbedMainPage ()
		{
            Instance = this;

            InitializeComponent ();

           
            //var profilePage = new NavigationPage(new ProfilePage());
            //profilePage.Icon = "Icon.png";
            //profilePage.Title = "Schedule";
            //
            //var mapPage = new NavigationPage(new MapPage());
            //mapPage.Icon = "Icon.png";
            //mapPage.Title = "Schedule";
            //
            //var managementPage = new NavigationPage(new ManagementPage());
            //managementPage.Icon = "Icon.png";
            //managementPage.Title = "Schedule";
            //
            //
            //Children.Add(profilePage);
            //Children.Add(mapPage);
            //Children.Add(managementPage);


            SelectedItem = Children[1];
            //NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}