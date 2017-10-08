using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace goParty.ViewModels
{
    public class TabbedContentPageViewModel : BaseViewModel
    {
        public ObservableRangeCollection<ContentView> _items = new ObservableRangeCollection<ContentView>();

        public ObservableRangeCollection<ContentView> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value, "Items"); }
        }

        CustomTabView _TabView;
        public CustomTabView TabView
        {
            get { return _TabView; }
            set { SetProperty(ref _TabView, value, "TabView");TabView.ItemSelected += RefreshPage;}
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


        async Task RefreshPage(object sender, int index)
        {
            App.currentPosition = await LocationHelper.GetLocation();
            //CardListView senderListView = (CardListView)sender;

            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            ICloudTable<PartyDetails> table = cloudService.GetTable<PartyDetails>();
            ICollection<PartyDetails> parties = await cloudService.RetreivePartiesWithinRange(App.currentPosition.Longitude, App.currentPosition.Latitude, Constants.defaultSearchRange, index);

            ObservableRangeCollection<CardListItem> sortedItems = new ObservableRangeCollection<CardListItem>();

            foreach (var item in parties)
            {
                CardListItem temp = new CardListItem()
                {
                    partyDetailsItem = new PartyDetailsItem(item)
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
