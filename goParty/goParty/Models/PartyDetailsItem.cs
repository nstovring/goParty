using goParty.Abstractions;
using goParty.Helpers;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Azure.Documents.Spatial;
using goParty.Models.APIModels;
using Xamarin.Auth;
using System.IO;
using System.Linq;
using Xamarin.Forms.Maps;

namespace goParty.Models
{
    public class PartyDetailsItem : PartyDetails, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Command joinPartyCmd;
        public Command JoinPartyCommand => joinPartyCmd ?? (joinPartyCmd = new Command(async () => await ExecuteJoinPartyCommand().ConfigureAwait(false)));

        public int index;
        bool _isJoinButtonActive = true;
        bool _isThisUserAttending = false;
        bool _isThisUsersFriendsAttending = false;
        bool _isThisUserHosting = false;
        bool _propIsBusy;
        string _joinButtonLabel = null;

        public bool IsBusy
        {
            get { return _propIsBusy; }
            set { SetProperty(ref _propIsBusy, value, "isBusy"); }
        }

        public bool isJoinButtonActive
        {
            get { return _isJoinButtonActive; }
            set { SetProperty(ref _isJoinButtonActive, value, "isJoinButtonActive"); }
        }

        public bool isThisUserAttending
        {
            get { return _isThisUserAttending; }
            set { SetProperty(ref _isThisUserAttending, value, "isThisUserAttending"); }
        }

        public bool isThisUsersFriendsAttending
        {
            get { return _isThisUsersFriendsAttending; }
            set { SetProperty(ref _isThisUsersFriendsAttending, value, "isThisUsersFriendsAttending"); }
        }

        public bool isThisUserHosting
        {
            get { return _isThisUserHosting; }
            set { SetProperty(ref _isThisUserHosting, value, "isThisUserHosting"); }
        }


        public string joinButtonLabel
        {
            get { return _joinButtonLabel; }
            set { SetProperty(ref _joinButtonLabel, value, "joinButtonLabel"); }
        }

        public ImageSource _pictureImageSource;

        public ImageSource pictureImageSource
        {
            get { return _pictureImageSource; }
            set { SetProperty(ref _pictureImageSource, value, "pictureImageSource"); }
        }

        public ImageSource _hostPictureImageSource;

        public ImageSource hostPictureImageSource
        {
            get { return _hostPictureImageSource; }
            set { SetProperty(ref _hostPictureImageSource, value, "hostPictureImageSource"); }
        }


        string _ageRange;

        public string AgeRange
        {
            get { return _ageRange; }
            set { SetProperty(ref _ageRange, value, "ageRange"); }
        }

        string _type;

        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value, "Type"); }
        }


        //public string Hostpicture
        //{
        //    get { return hostpicture; }
        //    set { SetProperty(ref hostpicture, value, "Hostpicture"); }
        //}

        PartyDetails valueSource;

        public PartyDetailsItem(PartyDetails valueSource)
        {
            this.valueSource = valueSource;
            //InitializeCard(valueSource);
        }

        public async Task InitializeCard()
        {
            hostpicture = valueSource.hostpicture;
            userId = valueSource.userId;
            partyId = valueSource.partyId;
            ageMin = valueSource.ageMin;
            ageMax = valueSource.ageMax;
            picture = valueSource.picture;
            title = valueSource.title;
            description = valueSource.description;
            when = valueSource.when;
            where = valueSource.where;
            Type = Constants.PartyTypes[valueSource.type];
            type = valueSource.type;
            lon = valueSource.lon;
            latt = valueSource.latt;
            Id = valueSource.Id;
            documentDBId = valueSource.documentDBId;
            price = valueSource.price;
            rating = valueSource.rating;
            //location = new Microsoft.Azure.Documents.Spatial.Point(lon,latt);
            AgeRange = ageMin.ToString() + "-" + ageMax.ToString();

            if (isThisUserAttending == true)
            {
                joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.RequestSent];
            }
            else if (isThisUserHosting == true)
            {
                joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.CancelEvent];
            }
            else
            {
                joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.JoinParty];
            }
            pictureImageSource = await LoadImage(picture);
        }

        public async Task<ImageSource> LoadImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return null;
            if (imagePath.Length > 10 && Uri.IsWellFormedUriString(imagePath,UriKind.RelativeOrAbsolute))
            {
                ImageHelper.ImageHelperItem item = ImageHelper.LoadedImages.FirstOrDefault(x => x.imageId == picture);
                ImageSource img = item?.image;
                return  img ?? await AzureStorage.LoadImage(picture);
            }
            else
            {
                ImageHelper.ImageHelperItem item = ImageHelper.LoadedImages.FirstOrDefault(x => x.imageId == picture);
                ImageSource img;
                if (item == null)
                {
                    img = ImageSource.FromFile(picture);
                    ImageHelper.LoadedImages.Add(new ImageHelper.ImageHelperItem { image = img, imageId = picture });
                    return  img;
                }
                return item.image;
            }
        }

        public async Task ExecuteJoinPartyCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            joinButtonLabel = null;
           
            try
            {
                var loginProvider = DependencyService.Get<ILoginProvider>();

                //TODO Optimize
                string customerid;
                //loginProvider.RetreiveAccountFromSecureStore().Properties.TryGetValue(Constants.stripeAccountIdPropertyName,out customerid);
                string userTableid = loginProvider.RetrieveTableIdFromSecureStore();

                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                ICloudTable<AttendeeDetails> Table = cloudService.GetTable<AttendeeDetails>();

                AttendeeDetails attendeeDetails = new AttendeeDetails
                {
                    userId = userTableid,
                    partyId = Id, //Actually tableid
                    paid = false,
                    accepted = false,
                    chargeId = "  ",
                };
            
                await Table.CreateItemAsync(attendeeDetails);
                //TODO register for notifications
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Login] Error = {ex.Message}");
            }
            finally
            {
                joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.RequestSent];
                Console.WriteLine("Join Party Command Executed");
                IsBusy = false;
            }
        }


        protected void SetProperty<T>(ref T store, T value, string propName, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(store, value))
                return;
            store = value;
            if (onChanged != null)
                onChanged();
            OnPropertyChanged(propName);
        }

        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged == null)
                return;
            if(propName == "isThisUserHosting")
            {
                if (isThisUserHosting == true)
                {
                    joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.CancelEvent];
                }
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
