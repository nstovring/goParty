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
    [Activity(Label = "goParty", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity instance;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            instance = this;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            var loginProvider = (DroidLoginProvider)DependencyService.Get<ILoginProvider>();
            loginProvider.Init(this);


            LoadApplication(new goParty.App());
        }
    }
}

