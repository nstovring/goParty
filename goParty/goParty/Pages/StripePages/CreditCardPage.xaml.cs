using goParty.Abstractions;
using goParty.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreditCardPage : ContentPage
	{
		public CreditCardPage ()
		{
			InitializeComponent ();
            BindingContext = new CreditCardViewModel();
        }
    }
}