using goParty.Abstractions;
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
    public partial class AttendeePage : TabbedPage
    {
        public AttendeePage ()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage == null)
                return;
            ((IRefreshable)CurrentPage).Refresh();
        }

    }
}