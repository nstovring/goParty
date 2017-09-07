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
        Command goToAttendeeViewCmd;
        public Command GoToAttendeeViewCommand => goToAttendeeViewCmd ?? (goToAttendeeViewCmd = new Command(() => ExecuteGoToSenderViewCommand(attendeeView, 1)));

        Command goToCreatePartyViewCmd;
        public Command GoToCreatePartyViewCommand => goToCreatePartyViewCmd ?? (goToCreatePartyViewCmd = new Command(async () => await ExecuteGoToCreatePartyPageCommand()));

        private async Task ExecuteGoToCreatePartyPageCommand()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new CreatePartyPage());
        }

        private async void ExecuteGoToSenderViewCommand(object sender, float e)
        {
            
            //if (mainPageVisible)
            //{
            //    await senderView.TranslateTo(-App.ScreenWidth, 0, 250, Easing.Linear);
            //    TabbedMainPage.Instance.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
            //
            //    mainPageVisible = !mainPageVisible;
            //}
            //else
            //{
            //    await senderView.TranslateTo(App.ScreenWidth, 0, 250, Easing.Linear);
            //    TabbedMainPage.Instance.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(true);
            //    mainPageVisible = !mainPageVisible;
            //}
        }


        public AbsoluteLayout MainLayout;
        CreatePartyView createPartyView;
        AtendeeView attendeeView;
        double createPartyXTranslation = App.ScreenWidth;

        bool mainPageVisible = true;
        public ManagementPageViewModel(AbsoluteLayout layout)
        {
            MainLayout = layout;
            Title = Constants.managePartyPageTitle;

            createPartyView = new CreatePartyView();
            createPartyView.createPartyViewModel.Tapped += ExecuteGoToSenderViewCommand;
            AbsoluteLayout.SetLayoutBounds(createPartyView, new Rectangle(createPartyXTranslation, 0, App.ScreenWidth, App.ScreenHeight));
            AbsoluteLayout.SetLayoutFlags(createPartyView, AbsoluteLayoutFlags.None);


            attendeeView = new AtendeeView();
            attendeeView.viewModel.Tapped += ExecuteGoToSenderViewCommand;
            AbsoluteLayout.SetLayoutBounds(attendeeView, new Rectangle(createPartyXTranslation, 0, App.ScreenWidth, App.ScreenHeight));
            AbsoluteLayout.SetLayoutFlags(attendeeView, AbsoluteLayoutFlags.None);


            MainLayout.Children.Add(createPartyView);
            MainLayout.Children.Add(attendeeView);

        }

    }
}
