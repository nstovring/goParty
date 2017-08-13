using goParty.Abstractions;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using goParty.Pages;
using Xamarin.Forms;
using goParty.Helpers;
namespace goParty
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();
            MainPage = new EntryPage();
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
	}
}
