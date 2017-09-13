using goParty.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Collections.Specialized;
using goParty.Abstractions;

namespace goParty.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AttendeePage : Xamarin.Forms.TabbedPage
    {
		public AttendeePage ()
		{
			InitializeComponent ();
            BindingContext = new AttendeePageViewModel();
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
        }

        protected override void OnPagesChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnPagesChanged(e);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage == null)
                return;
            IRefreshable RefreshablePage = CurrentPage as IRefreshable;
            RefreshablePage.Refresh();
        }


    }
}