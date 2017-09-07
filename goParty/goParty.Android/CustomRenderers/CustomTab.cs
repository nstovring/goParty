
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

[assembly: ExportRenderer(typeof(TabbedMainPage), typeof(CustomTab))]
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
                //for (int i = 0; i < layout.TabCount; i++)
                //{
                //    var tab = layout.GetTabAt(i);
                //    var icon = tab.Icon;
                //    if (icon != null)
                //    {
                //        icon = Android.Support.V4.Graphics.Drawable.DrawableCompat.Wrap(icon);
                //        Android.Support.V4.Graphics.Drawable.DrawableCompat.SetTintList(icon, colors);
                //    }
                //}

                //ColorStateList colors = null;
                //if ((int)Build.VERSION.SdkInt >= 23)
                //{
                //    colors = Resources.GetColorStateList(Resource.Color.icon_tab, Forms.Context.Theme);
                //}
                //else
                //{
                //    colors = Resources.GetColorStateList(Resource.Color.icon_tab);
                //}
                //
                //for (int i = 0; i < layout.TabCount; i++)
                //{
                //    var tab = layout.GetTabAt(i);
                //    var icon = tab.Icon;
                //    if (icon != null)
                //    {
                //        icon = Android.Support.V4.Graphics.Drawable.DrawableCompat.Wrap(icon);
                //        Android.Support.V4.Graphics.Drawable.DrawableCompat.SetTintList(icon, colors);
                //    }
                //}

            }
        }

    }
}