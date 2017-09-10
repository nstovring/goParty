using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using goParty.Helpers;
using goParty.Models;
using goParty.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Threading.Tasks;
using goParty.Abstractions;

namespace goParty.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class PartyCarouselPageViewModel : BaseViewModel
    {
        public ObservableRangeCollection<PartyDetailsDBCarouselItem> Parties { get; set; }
        public ObservableRangeCollection<PartyDetailsDB> PartiesGrouped { get; set; }
        //public PartyDetailsDBCarouselItem selectedParty;
        public PartyCarouselPageViewModel()
        {
            Title = Constants.PartyCarouselViewModelTitle;
            Parties = new ObservableRangeCollection<PartyDetailsDBCarouselItem>();
            Parties.AddRange(AzurePartyManager.DefaultManager.CarouselItems);
        }
    }
}
