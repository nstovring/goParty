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
            SelectedItem = Children[1];
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}