using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Helpers;
using Xamarin.Auth;
using Microsoft.Azure.Search;
using goParty.Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace goParty.ViewModels
{
    public class EntryPageViewModel : BaseViewModel
    {
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
                await cloudService.RetreiveExtraDataFromCloud();
                await cloudService.RegisterForPushNotifications();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }
            finally
            {
                Application.Current.MainPage = new NavigationPage(new Pages.RootPage());
                IsBusy = false;
            }
        }

       
    }
}
