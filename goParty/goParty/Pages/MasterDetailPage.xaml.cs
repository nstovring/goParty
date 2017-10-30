using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
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
    public partial class RootMasterDetailPage : MasterDetailPage
    {
        public static RootMasterDetailPage Instance;

        public RootMasterDetailPage()
        {
            Instance = this;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            if (item.TargetType == typeof(EntryPage))
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                await cloudService.LogoutAsync();
                //Application.Current.MainPage = new EntryPage();
                MasterPage.ListView.SelectedItem = null;
                IsPresented = false;
                await Navigation.PopToRootAsync();
                return;
            }
            else
            {

                Detail = new NavigationPage(page);
                IsPresented = false;
                MasterPage.ListView.SelectedItem = null;
            }
        }
    }
}