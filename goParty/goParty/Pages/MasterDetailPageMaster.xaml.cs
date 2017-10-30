using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageMaster : ContentPage
    {
        public ListView ListView;
        public MasterDetailPageMaster()
        {
            BindingContext = new MasterDetailPageMasterViewModel();
            InitializeComponent();
            ListView = MenuItemsListView;
        }

        class MasterDetailPageMasterViewModel : BaseViewModel, INotifyPropertyChanged
        {
            public ObservableRangeCollection<MasterPageItem> MenuItems { get; set; }

            public string _userName;
            public int _rating;
            public string _userProfilePicture;


            public string UserName
            {
                get { return _userName; }
                set { SetProperty(ref _userName, value, "UserName"); }
            }

            public int Rating
            {
                get { return _rating; }
                set { SetProperty(ref _rating, value, "Rating"); }
            }
            public string UserProfilePicture
            {
                get { return _userProfilePicture; }
                set { SetProperty(ref _userProfilePicture, value, "UserProfilePicture"); }
            }
         

            public MasterDetailPageMasterViewModel()
            {
                MenuItems = MasterDetailPageManager.PageItems();
                UserName = App.UserDetails.name;
                Rating = App.UserDetails.rating;
                UserProfilePicture = App.UserDetails.picture;
            }

           
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}