using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using goParty.ViewModels;
namespace goParty.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreatePartyView : ContentView
	{
        public CreatePartyViewModel createPartyViewModel;
        public CreatePartyView ()
		{
            createPartyViewModel = new CreatePartyViewModel(this);
            BindingContext = createPartyViewModel;
            InitializeComponent();
		}
    }
}