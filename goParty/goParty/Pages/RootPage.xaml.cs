using goParty.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using goParty.Models;
namespace goParty.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RootPage : MasterDetailPage
	{
        static MasterDetailPage instance;

        private List<Page> detailPages;

        public RootPage ()
		{
            detailPages = new List<Page>();
            instance = this;

			InitializeComponent ();
            BindingContext = new RootPageViewModel();

            detailPages.Add(Detail);

            masterPage.ListView.ItemSelected += ListView_ItemSelected;
		}

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null && item.TargetType != Detail.GetType())
            {
                //Check if page already exists
                if (detailPages.Any(x => x.GetType().Equals(item.TargetType)))
                {
                    //Find the page and use it
                    foreach (Page page in detailPages)
                    {
                        if (page.GetType().Equals(item.TargetType))
                        {
                            Detail = page;
                            masterPage.ListView.SelectedItem = null;
                            IsPresented = false;
                            break;
                        }
                    }
                }
                else //If the page doesn't exist create it
                {
                    Page newPage = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    detailPages.Add(newPage);
                    Detail = newPage;
                    masterPage.ListView.SelectedItem = null;
                    IsPresented = false;
                }
                //Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                //masterPage.ListView.SelectedItem = null;
                //IsPresented = false;
            }
            else
            {
                IsPresented = false;
            }
        }
    }
}