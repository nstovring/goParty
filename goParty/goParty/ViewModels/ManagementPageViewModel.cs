using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Pages;
using goParty.Abstractions;
using goParty.Helpers;
using goParty.Views;

namespace goParty.ViewModels
{
    public class ManagementPageViewModel : BaseViewModel
    {
        Command goToAttendeeViewCmd;
        public Command GoToAttendeeViewCommand => goToAttendeeViewCmd ?? (goToAttendeeViewCmd = new Command(async () => await ExecuteGoToAttendeeViewCommand().ConfigureAwait(false)));

        private Task ExecuteGoToAttendeeViewCommand()
        {
            throw new NotImplementedException();
        }

        private async Task ExecuteGoToAttendeePageCommand()
        {
            await App.Instance.MainPage.Navigation.PushModalAsync(new AttendeePage());
        }

        Command goToCreatePartyViewCmd;
        public Command GoToCreatePartyViewCommand => goToCreatePartyViewCmd ?? (goToCreatePartyViewCmd = new Command( () => ExecuteGoToCreatePartyViewCommand(createPartyView,1)));

        private async void ExecuteGoToCreatePartyViewCommand(object sender, float e)
        {
            if (mainPageVisible)
            {
                await createPartyView.TranslateTo(-App.ScreenWidth, 0, 250, Easing.Linear);
                mainPageVisible = !mainPageVisible;
            }
            else
            {
                await createPartyView.TranslateTo(App.ScreenWidth, 0, 250, Easing.Linear);
                mainPageVisible = !mainPageVisible;
            }
            //throw new NotImplementedException();
        }


        public AbsoluteLayout MainLayout;
        CreatePartyView createPartyView;

        double createPartyXTranslation = App.ScreenWidth;

        bool mainPageVisible = true;
        public ManagementPageViewModel(AbsoluteLayout layout)
        {
            MainLayout = layout;
            Title = Constants.managePartyPageTitle;
            createPartyView = new CreatePartyView();
            createPartyView.createPartyViewModel.Tapped += ExecuteGoToCreatePartyViewCommand;
            AbsoluteLayout.SetLayoutBounds(createPartyView, new Rectangle(createPartyXTranslation, 0, App.ScreenWidth, App.ScreenHeight));
            AbsoluteLayout.SetLayoutFlags(createPartyView, AbsoluteLayoutFlags.None);


            MainLayout.Children.Add(createPartyView);
        }

    }
}
