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

namespace goParty
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }

    public partial class App : Application
	{

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        public App ()
		{
			InitializeComponent();
            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();
            //MainPage = new EntryPage();
            try
            {
                MainPage = new EntryPage();
            }catch(Exception ex)
            {
                Console.WriteLine($"[Load Error] Error = {ex.Message}");
            }
        }


        //protected override void OnActivated(IActivatedEventArgs args)
        //{
        //    if (args.Kind == ActivationKind.Protocol)
        //    {
        //        ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;
        //        Frame content = Window.Current.Content as Frame;
        //        if (content.Content.GetType() == typeof(MainPage))
        //        {
        //            content.Navigate(typeof(MainPage), protocolArgs.Uri);
        //        }
        //    }
        //    Window.Current.Activate();
        //    base.OnActivated(args);
        //}

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
