using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using goParty.Models;
using System.Diagnostics;
using goParty.Services;
using Plugin.Geolocator;
using goParty.Helpers;
using goParty.Abstractions;
using goParty.Views;

namespace goParty.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        //private List<Pin> _pins;
        private Map _map;
        public static List<PartyDetailsDBCarouselItem> parties = new List<PartyDetailsDBCarouselItem>();

        public MapPage()
        {
            Title = Constants.locatePartyPageTitle;
            InitializeComponent();
            try
            {
                _map = new Map()
                {
                    IsShowingUser = true,
                    HeightRequest = 100,
                    WidthRequest = 960,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
            }catch(Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }

            var absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutBounds(_map, new Rectangle(0, 0, App.ScreenWidth, App.ScreenHeight));
            AbsoluteLayout.SetLayoutFlags(_map, AbsoluteLayoutFlags.None);

            var filterView = new MapFilterView();
            AbsoluteLayout.SetLayoutBounds(filterView, new Rectangle(0, 0, App.ScreenWidth, App.ScreenHeight));
            AbsoluteLayout.SetLayoutFlags(filterView, AbsoluteLayoutFlags.None);

            absoluteLayout.Children.Add(_map);
            absoluteLayout.Children.Add(filterView);

            Content = absoluteLayout;
            InitializePage();
        }
        async void InitializePage()
        {
            await InitMap();
           
        }

        private async Task InitMap()
        {
            var position = new Position(55.669989, 12.572854);
            try
            {
                var locator = CrossGeolocator.Current;
                var pos = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(50));
                position = new Position(pos.Latitude, pos.Longitude);
                //Overriding to Copenhagen
                position = new Position(55.669989, 12.572854);
                Debug.WriteLine("Updated Position");
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error: " + error);
            }
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            await GetPartiesFromDataBase(position);

            PlacePoints();
            _map.PropertyChanged += async (o, e) =>
            {
                await RefreshPartiesFromDataBase();
            };
        }

        Position mapCenterPosition;
        double zoom;
        AzurePartyManager manager;

        private async Task GetPartiesFromDataBase(Position position)
        {
            manager = AzurePartyManager.DefaultManager;
            await manager.DeleteAllBuggedPartiesAsync();
            //parties = await manager.GetAllPartiesAsync();
            parties = await manager.GetCarouselItemsAsync(
                await manager.GetPartiesFromLocationAsync(
                    position.Latitude,
                    position.Longitude, 
                    30000));

            if (parties == null || !parties.Any())
            {
                ICloudTable<PartyDetails> Table;
                ICloudService cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                Table = cloudService.GetTable<PartyDetails>();
                ICollection<PartyDetails> tempPartyDetails = await Table.ReadAllItemsAsync();
            
                List<PartyDetailsDB> tempPartyDetailsDB = new List<PartyDetailsDB>();
                foreach(PartyDetails deet in tempPartyDetails)
                {
                    PartyDetailsDB tempDeetDB = new PartyDetailsDB {
                        title = deet.title,
                        description = deet.description,
                        partyId = deet.partyId,
                        userId = deet.userId,
                        picture = deet.picture,
                        ageMax = deet.ageMax,
                        ageMin = deet.ageMin,
                        rating = deet.rating,
                        price = deet.price,
                        when = deet.when,
                        where = deet.where,
                        type = deet.type,
                        lon = deet.lon,
                        latt = deet.latt,
                        maxParticipants = deet.maxParticipants,
                        documentDBId = deet.Id,
                        location = new Microsoft.Azure.Documents.Spatial.Point(deet.lon, deet.latt)
                    };
                    tempPartyDetailsDB.Add(tempDeetDB);
                }
            
                await Task.WhenAll(tempPartyDetailsDB.Select(q => manager.InsertItemAsync(q)));
                await manager.DeleteAllBuggedPartiesAsync();
                parties = await manager.GetCarouselItemsAsync(await manager.GetAllPartiesAsync());
            }

        }

        private async Task RefreshPartiesFromDataBase()
        {
            manager = AzurePartyManager.DefaultManager;
            mapCenterPosition = _map.VisibleRegion.Center;
            zoom = _map.VisibleRegion.Radius.Kilometers;
            parties = await manager.GetCarouselItemsAsync(
                await manager.GetPartiesFromLocationAsync(
                    mapCenterPosition.Latitude, 
                    mapCenterPosition.Longitude, 
                    zoom * 1000 * 2));
            PlacePoints();
        }

        private void PlacePoints()
        {
            if (parties == null || parties.Count < 1)
            {
                return;
            }

            int index = 0;
            foreach (PartyDetailsDBCarouselItem party in parties)
            {
                var position = new Position(party.latt, party.lon);
                var pin = new Pin()
                {
                    Type = party.isThisUserHosting ? PinType.SavedPin : PinType.Generic,
                    Position = position,
                    Label = party.title,
                    Address = party.where
                };
                pin.Clicked += (sender, args) => {
                    GoToParties(sender, args,  party);
                };
                if (!_map.Pins.Contains(pin))
                {
                    _map.Pins.Add(pin);
                }
                index++;
            }
        }


        public async void GoToParties(Object sender, EventArgs args, PartyDetailsDBCarouselItem selectedParty)
        {
            //PartyHelper.SortPartyByIndex(index);
            manager.SortCarouselPartiesToIndex(selectedParty.index);
            Debug.WriteLine("Pin Pressed");
            var carouselPage = new PartyCarouselPage();
            await Navigation.PushModalAsync(carouselPage, false);
        }
    }
}