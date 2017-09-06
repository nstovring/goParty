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
        const int animLength = 250;
        public List<UserCardView> cards = new List<UserCardView>();

        public int UserCardCount = 0;
        public double cardHeight = 150;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.ICollection), typeof(CardListView), null,
            propertyChanged: OnItemsSourcePropertyChanged);

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CardListView)bindable).Refresh();
        }

        public ObservableRangeCollection<AttendeeListItem> ItemsSource
        {
            get
            {
                return (ObservableRangeCollection<AttendeeListItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public CardListView()
        {
            InitializeComponent();
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
            ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            if (ItemsSource == null || ItemsSource.Count <= 0)
                return;

            PopulateView();
        }

        public async void OnChildSwiped(object sender, bool e)
        {
            if (e == false)
            {
                cards = OrderCardsByIndex(cards);

                UserCardView user = (UserCardView)sender;

                for (int i = user.index + 1; i < UserCardCount; i++)
                {
                    UserCardView card = cards[i];
                    card.index--;
                }

                cards[user.index].index = UserCardCount - 1;

                cards = OrderCardsByIndex(cards);
                OffsetCards();
            }
            else
            {

            }
        }

        public List<UserCardView> OrderCardsByIndex(List<UserCardView> cards)
        {
            UserCardView[] temp = new UserCardView[cards.Count];

            for (int i = 0; i < UserCardCount; i++)
            {
                UserCardView card = cards[i];
                temp[card.index] = card;
            }

            return temp.ToList();
        }


        public async void PopulateView()
        {
            var tcs = new TaskCompletionSource<List<UserCardView>>();
            ListAbsoluteLayout.Children.Clear();
            UserCardCount = ListAbsoluteLayout.Children.Count;
            foreach (var item in ItemsSource)
            {
                UserCardView cardViewItem = new UserCardView(item);
                cardViewItem.index = UserCardCount;
                cardViewItem.Swiped += OnChildSwiped;
                cardViewItem.Tapped += OffsetCardsByHeight;
                cardViewItem.HeightRequest = cardHeight;
                ListAbsoluteLayout.Children.Add(cardViewItem);
                cards.Add(cardViewItem);
                UserCardCount++;
            }
            tcs.SetResult(cards);

            await tcs.Task;
            OffsetCards();
        }

        public int margin = 5;

        public void OffsetCards()
        {
            double currentOffset = margin;
            foreach (var card in cards)
            {
                card.TranslateTo(0, (card.HeightRequest * card.index) + margin * card.index, animLength, Easing.SpringIn);
            }
        }

        public void OffsetCardsByHeight(object sender, float requestedHeight)
        {
            UserCardView user = (UserCardView)sender;
            double offset = requestedHeight - user.Height;

            for (int i = user.index + 1; i < UserCardCount; i++)
            {
                UserCardView card = cards[i];
                card.TranslateTo(0, card.TranslationY + offset, animLength, Easing.Linear);
            }
        }
    }
}