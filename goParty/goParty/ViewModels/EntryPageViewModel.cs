using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Helpers;
using Xamarin.Auth;
namespace goParty.ViewModels
{
    public class EntryPageViewModel : BaseViewModel
    {

        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() =>
                {
                    //show your app page
                    var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
                    masterDetailPage.Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPage)));
                    masterDetailPage.IsPresented = false;

                    var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                    cloudService.RegisterForPushNotifications();
                    Application.Current.MainPage = new NavigationPage(new Pages.TaskList());
                });
            }
        }

        public EntryPageViewModel()
        {
            Title = "Task List";
        }

        Command loginCmd;
        public Command LoginCommand => loginCmd ?? (loginCmd = new Command(async () => await ExecuteLoginCommand().ConfigureAwait(false)));

        async Task ExecuteLoginCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                await cloudService.LoginAsync();
                await cloudService.RegisterForPushNotifications();
                Application.Current.MainPage = new NavigationPage(new Pages.TaskList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void FinalizeLoginCommand()
        {
            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            await cloudService.RegisterForPushNotifications();
            Application.Current.MainPage = new NavigationPage(new Pages.TaskList());
        }
    }
}
