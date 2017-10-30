using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Views.RatingView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingView : ContentView
    {
        int maxRating = 5;

        public static readonly BindableProperty RatingProperty =
          BindableProperty.Create(nameof(Rating), typeof(int), typeof(RatingView), 0,
          propertyChanged: OnRatingPropertyChanged);

        public int Rating
        {
            get
            {
                return (int)GetValue(RatingProperty);
            }
            set
            {
                SetValue(RatingProperty, value);
            }
        }



        private static void OnRatingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
             ((RatingView)bindable).ConvertRatingToStars();
        }

        public RatingView()
        {
            InitializeComponent();
        }

        public double size = 30;

        public void ConvertRatingToStars()
        {
            RatingStack.Children.Clear();
            for (int i = 0; i < maxRating; i++)
            {
                if (i < Rating)
                {
                    Image star = new Image()
                    {
                        Source = "fullstar.png",
                        //HeightRequest = size,
                        //WidthRequest = size
                    };
                    RatingStack.Children.Add(star);
                }
                else
                {
                    Image star = new Image()
                    {
                        Source = "emptystar.png",
                        //HeightRequest = size,
                        //WidthRequest = size
                    };
                    RatingStack.Children.Add(star);
                }
            }
        }
    }
}