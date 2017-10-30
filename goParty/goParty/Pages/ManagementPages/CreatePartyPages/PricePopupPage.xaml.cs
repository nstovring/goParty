using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using goParty.Pages;
using Rg.Plugins.Popup.Pages;
using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using goParty.Pages;
using goParty.Views;
using Xamarin.Forms.Xaml;
using goParty.ViewModels;
using Rg.Plugins.Popup.Services;

namespace goParty.Pages.ManagementPages.CreatePartyPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PricePopupPage : PopupPage
    {
        PartyDetails partyDetails;
        public string price;
        public PricePopupPage ()
		{
            CloseWhenBackgroundIsClicked = true;
            Padding = 20;
            if (CreatePartyViewModel._partyDetails != null)
            {
                partyDetails = CreatePartyViewModel._partyDetails;
            }
            else
            {
                throw new NullReferenceException("No partyDetails assigned to Price Popup page page");
            }

            InitializeComponent ();
            //BindingContext = this;
		}


        private void CloseWindowButtonPressed(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private void AcceptWindowButtonPressed(object sender, EventArgs e)
        {
            //int p;
            //int.TryParse(price,out p);
            //partyDetails.price = p;
            OnBackButtonPressed();
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            PopupNavigation.PopAsync(true);
            //return base.OnBackButtonPressed();
            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            return base.OnBackgroundClicked();
        }
    }
}