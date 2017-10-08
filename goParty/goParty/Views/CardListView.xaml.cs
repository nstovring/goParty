using goParty.Helpers;
using goParty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardListView : ContentView
    {

        string _propTitle;
        bool _propIsBusy;
        public int index = 0;

        public string Title
        {
            get { return _propTitle; }
            set { SetProperty(ref _propTitle, value, "Title"); }
        }

        public bool IsBusy
        {
            get { return _propIsBusy; }
            set { SetProperty(ref _propIsBusy, value, "IsBusy"); }
        }


        const int animLength = 250;
        public List<CardView> cards = new List<CardView>();

        public int UserCardCount = 0;
        public double cardHeight = 200;


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.ICollection), typeof(CardListView), null,
            propertyChanged: OnItemsSourcePropertyChanged);

        private static async void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            await ((CardListView)bindable).Refresh();
        }

        public ObservableRangeCollection<CardListItem> ItemsSource
        {
            get
            {
                return (ObservableRangeCollection<CardListItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public CardListView()
        {
            InitializeComponent();
            scrollView = CardScrollView;
            BindingContext = this;
        }

        //public delegate event OnRefresh
        // A delegate type for hooking up change notifications.
        public delegate Task RefreshEventHandler(object sender, int index);
        public event RefreshEventHandler Refreshed;

        private async void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await Refresh();
            BindingContext = this;
        }

        public PartyCardView partyCardView;

        public async void TransitionToCardDetails(CardView sender, TappedEventArgs e)
        {
            //Get card details view
            
            //Fill content

            //Animate image
        }

        Command refreshCmd;
        public Command RefreshCommand => refreshCmd ?? (refreshCmd = new Command(async () => await Refresh().ConfigureAwait(false)));

        public async Task Refresh()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Refreshed?.Invoke(this, index);
                ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
                ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
                if (ItemsSource == null || ItemsSource.Count <= 0)
                    return;

                await PopulateView();
            }catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Refresh Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnChildSwiped(object sender, bool e)
        {
            //Swiped left
            if (e == false)
            {
                cards = OrderCardsByIndex(cards);

                CardView user = (CardView)sender;
                //await user.attendee.ExecuteDeclineAttendeeCommand();

                for (int i = user.index + 1; i < UserCardCount; i++)
                {
                    CardView card = cards[i];
                    card.index--;
                }

                cards[user.index].index = UserCardCount - 1;

                cards = OrderCardsByIndex(cards);
                //OffsetCards();
            }
            //Swiped Right
            else
            {
                cards = OrderCardsByIndex(cards);

                CardView user = (CardView)sender;

                //await user.attendee.ExecuteAcceptAttendeeCommand();
                for (int i = user.index + 1; i < UserCardCount; i++)
                {
                    CardView card = cards[i];
                    card.index--;
                }

                cards[user.index].index = UserCardCount - 1;

                cards = OrderCardsByIndex(cards);
                //OffsetCards();
            }
        }

        void RemoveCardFromList(CardListItem view)
        {
            //ListAbsoluteLayout.Children.Remove(view);
            ItemsSource.RemoveAt(view.index);

            PopulateView();
        }

        public List<CardView> OrderCardsByIndex(List<CardView> cards)
        {
            CardView[] temp = new CardView[cards.Count];

            for (int i = 0; i < UserCardCount; i++)
            {
                CardView card = cards[i];
                temp[card.index] = card;
            }

            return temp.ToList();
        }
        public static ScrollView scrollView;

        public async Task PopulateView()
        {
            var tcs = new TaskCompletionSource<List<CardView>>();
            ListStackLayout.Children.Clear();
            UserCardCount = ListStackLayout.Children.Count;
            foreach (var item in ItemsSource)
            {
                CardView cardViewItem = new CardView(item);
                cardViewItem.index = UserCardCount;
                cardViewItem.HeightRequest = cardHeight;
                cardViewItem.WidthRequest = App.ScreenWidth;

                ListStackLayout.Children.Add(cardViewItem);
                cards.Add(cardViewItem);
                UserCardCount++;
            }
            tcs.SetResult(cards);

            await tcs.Task;
            //OffsetCards();
            UpdateChildrenLayout();
        }

        public int margin = 5;

        protected void SetProperty<T>(ref T store, T value, string propName, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(store, value))
                return;
            store = value;
            if (onChanged != null)
                onChanged();
            OnPropertyChanged(propName);
        }


        //public void OnPropertyChanged(string propName)
        //{
        //    if (PropertyChanged == null)
        //        return;
        //    PropertyChanged(this, new PropertyChangedEventArgs(propName));
        //}
    }
}