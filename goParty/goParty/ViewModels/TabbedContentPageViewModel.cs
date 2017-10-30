using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace goParty.ViewModels
{
    public class TabbedContentPageViewModel : BaseViewModel
    {
        public ObservableRangeCollection<ContentView> _items = new ObservableRangeCollection<ContentView>();
        public Position searchPosition;
        int selectedTab = 0;

        public ObservableRangeCollection<ContentView> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value, "Items"); }
        }

        CustomTabView _TabView;
        public CustomTabView TabView
        {
            get { return _TabView; }
            set { SetProperty(ref _TabView, value, "TabView");TabView.ItemSelected += RefreshPage; TabView.Searched += Tabview_Searched; }
        }


        public TabbedContentPageViewModel()
        {
            Initialize();
        }

        public static PartyDetailsItem selectedDetailsItem;

        public async void Initialize()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                //Subscribe to filterchanged events
                FilterSettings.FilterChanged += OnFilterChanged;    
                //Do not await anything here
                searchPosition = App.CurrentPosition;
                
                _items = new ObservableRangeCollection<ContentView>();
                int index = 0;
                foreach (var type in Constants.PartyTypes)
                {
                    CardListView cardListView = new CardListView();
                    cardListView.index = index;
                    cardListView.Title = type;
                    cardListView.Refreshed += RefreshPage;
                    _items.Add(cardListView);
                    index++;
                }

                await ((CardListView)_items[0]).Refresh();
               
            }catch(Exception ex)
            {
                Console.WriteLine("[$] Error Occured! " + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnFilterChanged(object sender)
        {
            searchPosition = FilterSettings.SearchPosition;
            //throw new NotImplementedException();
        }

        private async Task Tabview_Searched(object sender, Position position)
        {
            searchPosition = position;
            await ((CardListView)_items[selectedTab]).Refresh();
        }


        async Task RefreshPage(object sender, int index)
        {
            selectedTab = index;
            //CardListView senderListView = (CardListView)sender;

            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            ICloudTable<PartyDetails> table = cloudService.GetTable<PartyDetails>();
            ICollection<PartyDetails> parties = await cloudService.RetreivePartiesWithinRange(searchPosition.Longitude, searchPosition.Latitude, Constants.defaultSearchRange, index);

            ObservableRangeCollection<CardListItem> sortedItems = new ObservableRangeCollection<CardListItem>();

            foreach (var item in parties)
            {
                PartyDetailsItem tempItem = new PartyDetailsItem(item);
                await tempItem.InitializeCard();
                CardListItem temp = new CardListItem()
                {
                    partyDetailsItem = tempItem
                };
                sortedItems.Add(temp);
            }
            ((CardListView )_items[index]).ItemsSource = sortedItems;
        }


        void CreateTypePage(string type)
        {

        }
    }
}
