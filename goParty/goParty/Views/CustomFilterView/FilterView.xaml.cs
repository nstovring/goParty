using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Views.CustomFilterView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilterView : ContentView
	{
		public FilterView ()
		{
            TranslationX = App.ScreenWidth;
            TranslationY = -HeightRequest;
            //Opacity = 0;
            InitializeComponent ();
            Unfocused += FilterView_Unfocused;
            
        }

        private void FilterView_Unfocused(object sender, FocusEventArgs e)
        {
            TranslationX = App.ScreenWidth;
            TranslationY = -HeightRequest;
        }
    }
}