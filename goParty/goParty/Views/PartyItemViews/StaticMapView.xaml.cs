using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace goParty.Views.PartyItemViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StaticMapView : ContentView , IRefreshable
	{
        public string addresse
        {
            get
            {
                return (string)GetValue(AddresseProperty);
            }
            set
            {
                SetValue(AddresseProperty, value);
            }
        }

        public Microsoft.Azure.Documents.Spatial.Position position
        {
            get
            {
                return (Microsoft.Azure.Documents.Spatial.Position)GetValue(PositionProperty);
            }
            set
            {
                SetValue(PositionProperty, value);
            }
        }
        public double Lattitude
        {
            get
            {
                return (double)GetValue(LattitudeProperty);
            }
            set
            {
                SetValue(LattitudeProperty, value);
            }
        }
        public double Longtitude
        {
            get
            {
                return (double)GetValue(LongtitudeProperty);
            }
            set
            {
                SetValue(LongtitudeProperty, value);
            }
        }

        public static readonly BindableProperty AddresseProperty =
            BindableProperty.Create(nameof(addresse), typeof(string), typeof(StaticMapView), "",
            propertyChanged: OnItemsSourcePropertyChanged);

        public static readonly BindableProperty LattitudeProperty =
           BindableProperty.Create(nameof(Lattitude), typeof(double), typeof(double), 0.0,
           propertyChanged: OnItemsSourcePropertyChanged);

        public static readonly BindableProperty LongtitudeProperty =
           BindableProperty.Create(nameof(Longtitude), typeof(double), typeof(double), 0.0,
           propertyChanged: OnLongitudePropertyChanged);

        public static readonly BindableProperty PositionProperty =
           BindableProperty.Create(nameof(position), typeof(Microsoft.Azure.Documents.Spatial.Position), typeof(StaticMapView), null,
           propertyChanged: OnItemsSourcePropertyChanged);



        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //((StaticMapView)bindable).Refresh();
        }

        private static void OnLongitudePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((StaticMapView)bindable).Refresh();
        }

        public Task Refresh()
        {
            Position pos = new Position(Lattitude, Longtitude);
            PartyLocation.MoveToRegion(MapSpan.FromCenterAndRadius(pos,Distance.FromKilometers(1)));
            PartyLocation.InputTransparent = true;
            //PartyLocation.HasZoomEnabled = false;
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = pos,
                Label = " ",
                Address = "Join Event To See Address"
            };
            PartyLocation.Pins.Add(pin);
            return Task.CompletedTask;
        }

        public StaticMapView ()
		{
			InitializeComponent ();
        }
	}
}