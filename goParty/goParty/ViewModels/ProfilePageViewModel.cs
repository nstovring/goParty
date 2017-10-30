using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Pages;
using goParty.Pages.ProfileSubPages;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ProfilePageViewModel : BaseViewModel
    {
        string _profilePicture;
        public string profilePicture
        {
            get { return _profilePicture; }
            set { SetProperty(ref _profilePicture, value, "profilePicture"); }
        }
        string _name;
        public string name
        {
            get { return _name; }
            set { SetProperty(ref _name, value, "name"); }
        }

        float _rating;
        public float rating
        {
            get { return _rating; }
            set { SetProperty(ref _rating, value, "rating"); }
        }

       
        Command goToSettingsPageCmd;
        public Command GoToSettingsPageCommand => goToSettingsPageCmd ?? (goToSettingsPageCmd = new Command(async () => await ExecuteGoToSettingsPageCommand().ConfigureAwait(false)));

        private async Task ExecuteGoToSettingsPageCommand()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new ProfileSettingsPage());
        }

        Command goToEditProfilecmd;
        public Command GoToEditProfilePageCommand => goToEditProfilecmd ?? (goToEditProfilecmd = new Command(async () => await ExecuteGoToEditProfileCommand().ConfigureAwait(false)));

        private async Task ExecuteGoToEditProfileCommand()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new EditProfilePage());
        }


        public StackLayout stackLayout;
        public StackLayout stackLayoutHosting;

        AzurePartyManager azurePartyManager;
        public ProfilePageViewModel()
        {
            Title = Constants.profilePageTitle;
            profilePicture = App.UserDetails.picture;
            rating = App.UserDetails.rating;
            name = App.UserDetails.name;
            //PartiesAttending = new ObservableRangeCollection<PartyDetails>();
           // PartiesHosting = new ObservableRangeCollection<PartyDetails>();

            azurePartyManager = AzurePartyManager.DefaultManager;
            //QueryForPartiesAttending();

        }

        
    }
}
