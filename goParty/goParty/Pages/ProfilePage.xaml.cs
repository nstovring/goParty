using CarouselView.FormsPlugin.Abstractions;
using goParty.Helpers;
using goParty.Pages.ProfileSubPages;
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
			InitializeComponent();
            ProfilePageViewModel profilePageViewModel = new ProfilePageViewModel();
            
            BindingContext = profilePageViewModel;
            NavigationPage.SetHasNavigationBar(this, false);

            var myCarousel = new CarouselViewControl();

            myCarousel.ItemsSource = new ObservableRangeCollection<View>
            {
                new FavoritesView(), new CreditInformationView(), new FriendsView()
            };

            myCarousel.ShowIndicators = true;
            myCarousel.IndicatorsShape = IndicatorsShape.Circle;
            //myCarousel.ItemTemplate = new MyTemplateSelector(); //new DataTemplate (typeof(MyView));
            myCarousel.BackgroundColor = Color.White;
            myCarousel.Position = 0; //default
            myCarousel.InterPageSpacing = 10;
            myCarousel.Orientation = CarouselViewOrientation.Horizontal;

            ProfilePageGrid.Children.Add(myCarousel, 0, 3);
            Grid.SetColumnSpan(myCarousel, 2);
        }

        public class FavoritesView : ContentView
        {
            Label label = new Label()
            {
                Text = "Favorittes"
            };
            
            public FavoritesView()
            {
                Content = label;

            }

        }


        public class CreditInformationView : ContentView
        {
            Label label = new Label()
            {
                Text = "Credit "
            };
            public CreditInformationView()
            {
                Content = label;

            }

        }


        public class FriendsView : ContentView
        {
            Label label = new Label()
            {
                Text = "Friends"
            };
            public FriendsView()
            {
                Content = label;
            }

        }
    }
}