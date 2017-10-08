using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Views.CustomTab
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HeaderItem : ContentView
	{
        public string _Text = "TEMP";
        public int index;

        public string Text
        {
            get { return _Text; }
            set { SetProperty(ref _Text, value, "Text"); }
        }

        public HeaderItem(string text, int index)
        {
            Text = text;
            this.index = index;


            InitializeComponent();
            HeaderLabel.Text = _Text;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                // handle the tap
                OnTapped(this, index);
            };
            HeaderLabel.GestureRecognizers.Add(tapGestureRecognizer);
        }

        // A delegate type for hooking up change notifications.
        public delegate void TappedEventHandler(object sender, int e);
        public event TappedEventHandler Tapped;

        public void OnTapped(object sender, int index)
        {
            HeaderLabel.TextColor = Color.Orange;
            Tapped(this, index);
        }

        public void OnUnselected(object sender, int selectedItem)
        {
            if (index == selectedItem)
                return;
            HeaderLabel.TextColor = Color.Black;
        }

        protected void SetProperty<T>(ref T store, T value, string propName, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(store, value))
                return;
            store = value;
            if (onChanged != null)
                onChanged();
            OnPropertyChanged(propName);
        }
    }
}