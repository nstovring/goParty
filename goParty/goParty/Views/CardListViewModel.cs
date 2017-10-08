using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace goParty.Views
{
    public class CardListViewModel: BaseViewModel
    {
        public CardListViewModel()
        {
            Title = "Card List";
        }

        Command RefreshCommand;
    }
}
