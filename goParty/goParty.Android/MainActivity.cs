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
using Plugin.Permissions;
using System.Threading.Tasks;
using System.IO;
using Android.Content;
using SVG.Forms.Plugin.Droid;
using CarouselView.FormsPlugin.Android;
using ImageCircle.Forms.Plugin.Droid;
using AsNum.XFControls.Droid;
using Refractored.XamForms.PullToRefresh.Droid;
using Adapt.Presentation.AndroidPlatform;

namespace goParty.Droid
{
	[Activity (Label = "goParty", Icon = "@drawable/icon", Theme="@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity , IRequestPermissionsActivity
    {
        #region Fields
        private PresentationFactory _PresentationFactory;
        #endregion

        #region Events
        public event PermissionsRequestCompletedHander PermissionsRequestCompleted;
        #endregion

        protected override void OnCreate (Bundle bundle)
		{
            TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

            //Adapt Initialization
            var permissions = new Permissions(this);
            _PresentationFactory = new PresentationFactory(ApplicationContext, permissions);

            base.OnCreate (bundle);


            global::Xamarin.Forms.Forms.Init (this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            PullToRefreshLayoutRenderer.Init();
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            var loginProvider = (DroidLoginProvider)DependencyService.Get<ILoginProvider>();
            CarouselViewRenderer.Init();
            ImageCircleRenderer.Init();
            loginProvider.Init(this);
            AsNumAssemblyHelper.HoldAssembly();
            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;

            try
            {
                LoadApplication(new goParty.App());
                Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Login] Error = {ex.Message}");
            }
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        #region Public Overrides
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsRequestCompleted?.Invoke(requestCode, permissions, grantResults);
        }
        #endregion


        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}

