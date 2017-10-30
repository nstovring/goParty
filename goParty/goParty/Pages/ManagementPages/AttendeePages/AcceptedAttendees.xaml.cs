using System.Threading.Tasks;
using goParty.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Pages.ManagementPages.AttendeePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcceptedAttendees : ContentPage, IRefreshable
    {
        public AcceptedAttendees()
        {
            InitializeComponent();
            BindingContext = new AttendeesViewModel(wrapLayout, AttendeesViewModel.AttendeeViewType.Accepted);
        }

        public async Task Refresh()
        {
            await ((AttendeesViewModel)BindingContext).Refresh();
        }
    }
}