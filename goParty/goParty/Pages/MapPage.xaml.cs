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

namespace goParty.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        //private List<Pin> _pins;
        private Map _map;
        public static List<PartyDetails> parties = new List<PartyDetails>();


        public MapPage()
        {
            //this.rootPage = rootPage;
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

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(_map);
            Content = stack;
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializePage();
        }
        async void InitializePage()
        {
            await InitMap();
           
        }
        Position mapCenterPosition;
        double zoom;
        AzureDocumentManager manager;

        private async Task GetPartiesFromDataBase(Position position)
        {
            manager = AzureDocumentManager.DefaultManager;
            parties = await manager.GetPartiesFromLocationAsync(position.Latitude, position.Longitude, 300000);

            //if (!parties.Any())
            //{
            //    List<PartyDetails> deets = DummyPartyDetails.dummyPartyDetails();
            //    await Task.WhenAll(deets.Select(q => manager.InsertItemAsync(q)));
            //    parties = await manager.GetPartiesFromLocationAsync(position.Latitude, position.Longitude, 300000);
            //}

        }

        private async Task RefreshPartiesFromDataBase()
        {
            manager = AzureDocumentManager.DefaultManager;
            mapCenterPosition = _map.VisibleRegion.Center;
            zoom = _map.VisibleRegion.Radius.Kilometers;
            parties = await manager.GetPartiesFromLocationAsync(mapCenterPosition.Latitude, mapCenterPosition.Longitude, zoom * 1000 * 2);
            PlacePoints();
        }


        private async Task InitMap()
        {
            var position = new Position(55.669989, 12.572854);
            try
            {
                var locator = CrossGeolocator.Current;
                var pos = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(50));
                position = new Position(pos.Latitude, pos.Longitude);
                //Ovveriding to Copenhagen
                position = new Position(55.669989, 12.572854);
                Debug.WriteLine("Updated Position");
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error: " + error);
            }
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(_map);
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));
            Content = stack;


            await GetPartiesFromDataBase(position);

            PlacePoints();
            _map.PropertyChanged += async (o, e) =>
            {
                await RefreshPartiesFromDataBase();
            };
        }

        private void PlacePoints()
        {
            //_map.Pins.Clear();
            if (parties == null || parties.Count < 1)
            {
                return;
            }

            //PartyHelper.AddParty(parties);

            int index = 0;
            foreach (PartyDetails party in parties)
            {
                var position = new Position(party.latt, party.lon);
                var pin = new Pin()
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = party.title,
                    Address = party.where
                };
                pin.Clicked += (sender, args) => {
                    GoToParties(sender, args, index);
                };
                if (!_map.Pins.Contains(pin))
                {
                    _map.Pins.Add(pin);
                }
                index++;
            }
        }


        public void GoToParties(Object sender, EventArgs args, int index)
        {
            //PartyHelper.SortPartyByIndex(index);

            //Debug.WriteLine("Pin Pressed");
            //var carouselPage = new PartiesPage();
            //await Navigation.PushModalAsync(carouselPage, false);
        }
    }
}