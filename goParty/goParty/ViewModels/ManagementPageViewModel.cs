using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Pages;
using goParty.Abstractions;
using goParty.Helpers;
using goParty.Views;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
namespace goParty.ViewModels
{
    public class ManagementPageViewModel : BaseViewModel
    {

        Command goToPartyCarouselPagecmd;
        public Command GoToPartyCarouselPageCommand => goToPartyCarouselPagecmd ?? (goToPartyCarouselPagecmd = new Command(() => ExecuteGoToPartyCarouselPageCommand()));

        private void ExecuteGoToPartyCarouselPageCommand()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new PartyCarouselPage());
        }

        Command goToAttendeePageCmd;
        public Command GoToAttendeePageCommand => goToAttendeePageCmd ?? (goToAttendeePageCmd = new Command(async () => await ExecuteGoToAttendeePageCommand()));

        private async Task ExecuteGoToAttendeePageCommand()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AttendeePage());
        }

        Command goToCreatePartyViewCmd;
        public Command GoToCreatePartyViewCommand => goToCreatePartyViewCmd ?? (goToCreatePartyViewCmd = new Command(async () => await ExecuteGoToCreatePartyPageCommand()));

        private async Task ExecuteGoToCreatePartyPageCommand()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new CreatePartyPage());
        }

        public AbsoluteLayout MainLayout;
        double createPartyXTranslation = App.ScreenWidth;

        public ManagementPageViewModel(AbsoluteLayout layout)
        {
            MainLayout = layout;
            Title = Constants.managePartyPageTitle;
        }

    }
}
