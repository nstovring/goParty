using System;
using System.Collections.Generic;
using System.Text;
using goParty.ViewModels;
namespace goParty.Helpers
{
    static class ViewModelLocator
    {
        private static PartyCarouselPageViewModel partiesVM;
        public static PartyCarouselPageViewModel PartiesViewModel
        => partiesVM ?? (partiesVM = new PartyCarouselPageViewModel());
    }
}
