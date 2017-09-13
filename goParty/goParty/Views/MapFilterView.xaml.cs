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
	public partial class MapFilterView : ContentView
	{
        double filterYMovement = 0;
        bool extended = false;
        bool panning = false;

        public MapFilterView ()
		{
			InitializeComponent ();
            this.TranslateTo(0, App.ScreenHeight / 1.3, 100, Easing.Linear);
           
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            panning = true;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    HandleTouchStart();
                    break;
                case GestureStatus.Running:
                    HandleTouch((float)e.TotalY);
                    break;
                case GestureStatus.Completed:
                    HandleTouchEnd();
                    break;
            }
        }

        public async void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                await this.TranslateTo(0, App.ScreenHeight / 2.5, 100, Easing.Linear);
                extended = !extended;
            }
            else
            {
                await this.TranslateTo(0, App.ScreenHeight / 1.3, 100, Easing.Linear);
                extended = !extended;
            }
        }

        public async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            if (panning)
                return;
            if (!extended)
            {
                await this.TranslateTo(0, App.ScreenHeight / 2, 100, Easing.Linear);
                extended = !extended;
            }
            else
            {
                await this.TranslateTo(0, App.ScreenHeight / 1.3, 100, Easing.Linear);
                extended = !extended;
            }
        }

        private void HandleTouch(float totalY)
        {
            filterYMovement = totalY;
            TranslationY += filterYMovement;
        }

        private void HandleTouchStart()
        {
            filterYMovement = 0;
        }

        double filterTop =0;
        double filterMiddle = App.ScreenHeight / 2;
        double filterUpperBottom = App.ScreenHeight / 1.5;
        double filterBottom = App.ScreenHeight / 1.3;
        private async void HandleTouchEnd()
        {
            int touchSign = Math.Sign(filterYMovement); // -1 going up 1 going down
            switch (touchSign)
            {
                case -1:
                    if (TranslationY < filterMiddle)
                    {
                        await this.TranslateTo(0, filterTop, 100, Easing.Linear);
                        panning = false;
                    }
                    else if (TranslationY < filterBottom)
                    {
                        await this.TranslateTo(0, filterMiddle, 100, Easing.Linear);
                        panning = false;
                    }
                    else
                    {
                        await this.TranslateTo(0, filterBottom, 100, Easing.Linear);
                        panning = false;
                    }
                    break;
                case 1:
                    if (TranslationY < filterMiddle)
                    {
                        await this.TranslateTo(0, filterMiddle, 100, Easing.Linear);
                        panning = false;
                    }
                    else if (TranslationY > filterMiddle)
                    {
                        await this.TranslateTo(0, filterBottom, 100, Easing.Linear);
                        panning = false;
                    }
                    else
                    {
                        await this.TranslateTo(0, filterBottom, 100, Easing.Linear);
                        panning = false;
                    }
                    break;
            }

            //if (TranslationY < App.ScreenHeight / 2)
            //{
            //    await this.TranslateTo(0,0, 100, Easing.Linear);
            //    panning = false;
            //}
            //else if(TranslationY < App.ScreenHeight /1.5 && TranslationY > App.ScreenHeight / 1.3)
            //{
            //    await this.TranslateTo(0, App.ScreenHeight/2, 100, Easing.Linear);
            //    panning = false;
            //}
            //

            filterYMovement = 0;
        }
    }
}