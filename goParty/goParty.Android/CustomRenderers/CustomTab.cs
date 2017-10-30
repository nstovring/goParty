
using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Content.Res;
using goParty.Pages;
using goParty.Droid.CustomRenderers;
using goParty.Abstractions;

[assembly: ExportRenderer(typeof(AttendeePage), typeof(CustomTab))]
namespace goParty.Droid.CustomRenderers
{
    public class CustomTab : TabbedPageRenderer
    {
        bool setup;
        ViewPager pager;
        TabLayout layout;
        private const string COLOR = "#00796B";

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (setup)
                return;

            if (e.PropertyName == "Renderer")
            {
                pager = (ViewPager)ViewGroup.GetChildAt(0);
                layout = (TabLayout)ViewGroup.GetChildAt(1);
                setup = true;

                
                layout.SetBackgroundColor(Android.Graphics.Color.White);
                layout.SetTabTextColors(Android.Graphics.Color.Black, Android.Graphics.Color.Red);
               

            }
        }
        TabbedPage _page;


        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _page = (AttendeePage)e.NewElement;
                
                //((IRefreshable)_page.CurrentPage).Refresh();
                //e.NewElement).Refresh();
            }
            else
            {
                _page = (AttendeePage)e.OldElement;
            }

        }

    }
}