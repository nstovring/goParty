using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using goParty.Droid.Services;
using Xamarin.Forms;
using goParty.Abstractions;

namespace goParty.Droid
{
	[Activity (Label = "goParty", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);


            var loginProvider = (DroidLoginProvider)DependencyService.Get<ILoginProvider>();
            loginProvider.Init(this);


            LoadApplication (new goParty.App ());
		}
	}
}

