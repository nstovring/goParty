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

namespace goParty.Models
{
    public class PartyDetailsDBCarouselItem : PartyDetailsDB, INotifyPropertyChanged
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

        public PartyDetailsDBCarouselItem(PartyDetails valueSource)
        {
            userId = valueSource.userId;
            partyId = valueSource.partyId;
            ageMin = valueSource.ageMin;
            ageMax = valueSource.ageMax;
            picture = valueSource.picture;
            title = valueSource.title;
            description = valueSource.description;
            when = valueSource.when;
            where = valueSource.where;
            lon = valueSource.lon;
            latt = valueSource.latt;
            Id = valueSource.Id;
            documentDBId = valueSource.documentDBId;
            price = valueSource.price;
            location = new Microsoft.Azure.Documents.Spatial.Point(lon,latt);

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

            LoadImage();
        }

        public async void LoadImage()
        {
            if (picture.Length > 10)
            {
                ByteArrayToImageSource byteArrayToImageSource = new ByteArrayToImageSource();
                Byte[] imageByteArray = await AzureStorage.GetFileAsync(ContainerType.Image, picture);
                Byte[] resizedImageByteArray = await ImageResizer.ResizeImage(imageByteArray, 800, 533);
                pictureImageSource = byteArrayToImageSource.Convert(resizedImageByteArray, typeof(ImageSource), null, null) as ImageSource;
            }
            else
            {
                pictureImageSource = ImageSource.FromFile(picture);
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
                loginProvider.RetreiveAccountFromSecureStore().Properties.TryGetValue(Constants.stripeAccountIdPropertyName,out customerid);
                string userTableid = loginProvider.RetrieveTableIdFromSecureStore();

                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                ICloudTable<AttendeeDetails> Table = cloudService.GetTable<AttendeeDetails>();

                AttendeeDetails attendeeDetails = new AttendeeDetails
                {
                    userId = userTableid,
                    partyId = documentDBId, //Actually tableid
                    paid = false,
                    accepted = false,
                    chargeId = customerid,
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
