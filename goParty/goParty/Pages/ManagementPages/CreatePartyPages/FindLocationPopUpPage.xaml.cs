using Rg.Plugins.Popup.Pages;
using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using goParty.Pages;
using goParty.Views;
using Xamarin.Forms.Xaml;
using goParty.ViewModels;
using Rg.Plugins.Popup.Services;

namespace goParty.Pages.ManagementPages.CreatePartyPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindLocationPopUpPage : PopupPage
	{
        PartyDetails partyDetails;

		public FindLocationPopUpPage ()
		{
            positions = new List<Position>();
            CloseWhenBackgroundIsClicked = true;
            Padding = 20;
            if (CreatePartyViewModel._partyDetails != null)
            {
                partyDetails = CreatePartyViewModel._partyDetails;
            }
            else
            {
                throw new NullReferenceException("No partyDetails assigned to Findlocation page");
            }
			InitializeComponent ();
            Initialize();
        }

        async void Initialize()
        {
            try
            {
                await Search(partyDetails.where);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                await PopupNavigation.PopAsync();
                return;
            }

        }

        private async void SearchBarView_SearchButtonPressed(object sender, EventArgs e)
        {
            //SearchCmd.Execute(this);
            await ExecuteSearchCommand();
        }

        private void CloseWindowButtonPressed(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private void AcceptWindowButtonPressed(object sender, EventArgs e)
        {
            //Open new View which asks if your'se certain of you're choice
            if (positions.Count > 0)
            {
                partyDetails.latt = positions[0].Latitude;
                partyDetails.lon = positions[0].Longitude;
                partyDetails.where = SearchBarView.Text;
            }
            OnBackButtonPressed();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public List<Position> positions;
        public string _searchResult;

        Command searchCmd;
        Command SearchCmd => (searchCmd ?? (searchCmd = new Command(async () => await ExecuteSearchCommand())));
        Geocoder geocoder;
        Geocoder GeoCoder => geocoder ?? (geocoder = new Geocoder());

        public async Task ExecuteSearchCommand()
        {
            string searchString = SearchBarView.Text;
            await Search(searchString);
        }

        async Task Search(string address)
        {
            MapView.Pins.Clear();
            try
            {
                positions.Clear();
                Position position = await GetLocationFromAddresse(address);
                positions.Add(position);
                await GoToLocation(position);
            }catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                await PopupNavigation.PopAsync();
                return;
            }
        }

        async Task<Position> GetLocationFromAddresse(string address)
        {
            if (!string.IsNullOrWhiteSpace(address))
            {
                IEnumerable<Position> approximateLocations = await GeoCoder.GetPositionsForAddressAsync(address);

                foreach (var position in approximateLocations)
                {
                    _searchResult += position.Latitude + ", " + position.Longitude + "\n";
                    return position;
                }

                if (positions.Count < 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No Locations found", "OK");
                }
            }
            return await LocationHelper.GetLocation();
        }

        async Task GoToLocation(Position position)
        {
            if (position != null)
            {
                MapSpan mapSpan = new MapSpan(position, 0.010, 0.010);
                var aproximateAdresses = await GeoCoder.GetAddressesForPositionAsync(position);
                List<string> addresses = new List<string>();
                foreach (var item in aproximateAdresses)
                {
                    addresses.Add(item);
                }

                Pin newPin = new Pin
                {
                    Position = new Position(position.Latitude, position.Longitude),
                    Address = addresses[0],
                    Label = string.IsNullOrWhiteSpace(partyDetails.title) ? "Found Location" : partyDetails.title
                };
                MapView.Pins.Add(newPin);
                SearchBarView.Text = addresses[0];
                MapView.MoveToRegion(mapSpan);
            }
        }

        // Method for animation child in PopupPage
        // Invoced after custom animation end
        protected override Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(1);
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected override Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(0);
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            PopupNavigation.PopAsync(true);
            //return base.OnBackButtonPressed();
            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            return base.OnBackgroundClicked();
        }

    }
}