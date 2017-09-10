using goParty.Abstractions;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using goParty.Pages;
using Xamarin.Forms;
using goParty.Helpers;
using System.Threading.Tasks;
using goParty.Models;
using Xamarin.Auth;
using Microsoft.Azure.Search;
using Stripe;

namespace goParty
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }

    public partial class App : Application
	{
        public static double ScreenWidth;
        public static double ScreenHeight;

        public static UserDetails userDetails;
        public static Account account;
        public static SearchIndexClient UserDetailsUserIdSearchIndexClient;
        public static SearchIndexClient AttendeeUserIdSearchIndexClient;
        public static SearchIndexClient AttendeePartyIdSearchIndexClient;

        public static App Instance;

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        public App ()
		{
            Instance = this;
			InitializeComponent();
            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();
            ServiceLocator.Instance.Add<IStripeProvider, StripeService>();
            //MainPage = new EntryPage();
            try
            {
                MainPage = new EntryPage();
                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Load Error] Error = {ex.Message}");
            }
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        // Called by the back button in our header/navigation bar.
        public async void OnBackButtonPressed(object sender, EventArgs e)
        {
            await App.Instance.MainPage.Navigation.PopModalAsync();
        }
    }
}
