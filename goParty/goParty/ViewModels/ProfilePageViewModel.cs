using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Pages;
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

       
        Command goToCreditDetailsPageCmd;
        public Command GoToCreditDetailsPageCommand => goToCreditDetailsPageCmd ?? (goToCreditDetailsPageCmd = new Command(async () => await ExecuteGoToCreditDetailsPageCommand().ConfigureAwait(false)));

        private async Task ExecuteGoToCreditDetailsPageCommand()
        {
            await App.Instance.MainPage.Navigation.PushModalAsync(new CreditCardPage());
        }

        public StackLayout stackLayout;
        public StackLayout stackLayoutHosting;

        AzurePartyManager azurePartyManager;
        public ProfilePageViewModel()
        {
            Title = Constants.profilePageTitle;
            profilePicture = App.userDetails.picture;
            rating = App.userDetails.rating;
            name = App.userDetails.name;
            //PartiesAttending = new ObservableRangeCollection<PartyDetails>();
           // PartiesHosting = new ObservableRangeCollection<PartyDetails>();

            azurePartyManager = AzurePartyManager.DefaultManager;
            //QueryForPartiesAttending();

        }

        
    }
}
