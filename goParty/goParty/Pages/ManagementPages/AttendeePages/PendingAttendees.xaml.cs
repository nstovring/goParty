using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Pages.ManagementPages.AttendeePages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PendingAttendees : ContentPage, IRefreshable
    {
		public PendingAttendees ()
		{
			InitializeComponent ();
            BindingContext = new AttendeesViewModel(wrapLayout, AttendeesViewModel.AttendeeViewType.Pending);
        }

        public async Task Refresh()
        {
            await ((AttendeesViewModel)BindingContext).Refresh();
        }
    }
}