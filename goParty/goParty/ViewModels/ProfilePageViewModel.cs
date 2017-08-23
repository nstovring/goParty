using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ProfilePageViewModel : BaseViewModel
    {
        string _profilePicture;
        public string profilePicture
        {
            get { return _profilePicture; }
            set { SetProperty(ref _profilePicture, value, "profilePicture"); }
        }
        float _rating;
        public float rating
        {
            get { return _rating; }
            set { SetProperty(ref _rating, value, "rating"); }
        }

        public StackLayout stackLayout;
        public StackLayout stackLayoutHosting;

        public ObservableRangeCollection<PartyDetailsDBCarouselItem> partiesAttending { get; set; }
        public ObservableRangeCollection<PartyDetailsDBCarouselItem> partiesHosting { get; set; }
        AzurePartyManager azurePartyManager;
        public ProfilePageViewModel()
        {
            profilePicture = App.userDetails.picture;
            rating = App.userDetails.rating;
            partiesHosting = new ObservableRangeCollection<PartyDetailsDBCarouselItem>();
            partiesAttending = new ObservableRangeCollection<PartyDetailsDBCarouselItem>();

            azurePartyManager = AzurePartyManager.DefaultManager;
            QueryForPartiesAttending();

        }

        async void Refresh()
        {
            partiesHosting.Clear();
            partiesAttending.Clear();
        }  

        async void QueryForPartiesHosting()
        {
            partiesHosting.AddRange(await azurePartyManager.GetPartiesUserIsHostingAsync());
        }

        async void QueryForPartiesAttending()
        {
            ICloudService cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            ICloudTable<PartyDetails> Table = cloudService.GetTable<PartyDetails>();
            ICollection<PartyDetails> attendeeDetails = await Table.ReadAllItemsAsync();
            List<PartyDetailsDBCarouselItem> carouselItems = new List<PartyDetailsDBCarouselItem>();
            List<Image> carouselImages = new List<Image>();
            if (attendeeDetails == null || attendeeDetails.Count < 0)
                return;

            int i = 0;

            ICollection<PartyDetails> partiesAttending = new List<PartyDetails>();
            ICollection<PartyDetails> partiesHosting = new List<PartyDetails>();

            foreach (var item in attendeeDetails)
            {
                if(item.userId == App.userDetails.userId)
                {
                    partiesHosting.Add(item);
                }
                else
                {
                    partiesAttending.Add(item);
                }
            }
            //partiesHosting = attendeeDetails.Where(x => x.userId == App.userDetails.userId).ToList();
            //partiesAttending = attendeeDetails.Where(x => x.userId != App.userDetails.userId).ToList();

            foreach (var item in partiesAttending)
            {
                ImageSource src = await LoadImage(item.picture);
                Image image = new Image() {
                    Source = src,
                    VerticalOptions = LayoutOptions.Fill//Aspect = Aspect.AspectFit,
                };

                stackLayout.Children.Add(image);
            }

            foreach (var item in partiesHosting)
            {
                ImageSource src = await LoadImage(item.picture);
                Image image = new Image()
                {
                    Source = src,
                    VerticalOptions = LayoutOptions.Fill//Aspect = Aspect.AspectFit,
                };

                stackLayoutHosting.Children.Add(image);
            }
        }


        public async Task<ImageSource> LoadImage(string picture)
        {
            if (picture.Length > 10)
            {
                ByteArrayToImageSource byteArrayToImageSource = new ByteArrayToImageSource();
                return byteArrayToImageSource.Convert(await AzureStorage.GetFileAsync(ContainerType.Image, picture), typeof(ImageSource), null, null) as ImageSource;
            }
            else
            {
                return ImageSource.FromFile(picture);
            }
        }
    }
}
