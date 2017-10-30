using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using goParty.Pages;
using goParty.Views;
using goParty.Pages.ManagementPages.CreatePartyPages;
using Rg.Plugins.Popup.Services;

namespace goParty.ViewModels
{
    public class CreatePartyViewModel : BaseViewModel
    {
        #region Fields
        Stream partyHeaderImageStream;
        ImageSource _partyHeaderImageSource;
        Geocoder geoCoder;
        public static PartyDetails _partyDetails;

        bool _isPartyHeaderEnabled;
        bool _isJoinButtonActive = true;

        public string _searchText;
        public string _searchResult;
        string _joinButtonLabel = null;

        List<Position> positions;

        byte[] partyHeaderByteArray;

        bool pictureSelected = false;

        public Map ResultMap;
        #endregion
        #region Commands
        Command createPartyCmd;
        public Command CreatePartyCommand => createPartyCmd ?? (createPartyCmd = new Command(async () => await ExecuteCreatePartyCommand().ConfigureAwait(false)));

        Action evaluatePartyCmd;
        public Action EvaluatePartyCommand => evaluatePartyCmd ?? (evaluatePartyCmd = new Action(() => ExecuteEvaluatePartyCommand()));

        Command replacePartyImageCmd;
        public Command ReplacePartyImageCmd => replacePartyImageCmd ?? (replacePartyImageCmd = new Command(async () => await SelectImageCommand().ConfigureAwait(false)));

        Command findLocationCmd;
        public Command ChooseLocationCmd => findLocationCmd ?? (findLocationCmd = new Command(async () => await ExecuteFindLocationCommand().ConfigureAwait(false)));


        Command choosePriceCmd;
        public Command ChoosePriceCmd => choosePriceCmd ?? (choosePriceCmd = new Command(async () => await ExecuteChoosePriceCommand().ConfigureAwait(false)));

        private void ExecuteEvaluatePartyCommand()
        {
            if ((!string.IsNullOrWhiteSpace(_partyDetails.description) && !_partyDetails.description.Equals(Constants.descriptionPlaceholder)) &&
                (!string.IsNullOrWhiteSpace(_partyDetails.title) && !_partyDetails.title.Equals(Constants.titlePlaceholder)) &&
                (!string.IsNullOrWhiteSpace(_partyDetails.where) && !_partyDetails.where.Equals(Constants.wherePlaceholder)) &&
                (pictureSelected))
            {
                isJoinButtonActive = true;
            }
            else
            {
                isJoinButtonActive = false;
            }
        }

        private async Task ExecuteCreatePartyCommand()
        {
            if (IsBusy)
                return;
            joinButtonLabel = "";
            IsBusy = true;
            try
            {
                ImageSource partyHeaderSource = partyHeaderImageSource;

                string headerImagePath;

                headerImagePath = await AzureStorage.UploadFileAsync(ContainerType.Image, new MemoryStream(partyHeaderByteArray));

                Debug.WriteLine("AzureStorage Succes");

                //TODO Make sure user information is transferred to theese values
                partyDetails.picture = headerImagePath ?? " ";
                partyDetails.partyId = Guid.NewGuid().ToString();
                partyDetails.userId = App.UserDetails.userId ?? " ";


                AzurePartyManager manager = AzurePartyManager.DefaultManager;
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();

                ICloudTable<PartyDetails> Table = cloudService.GetTable<PartyDetails>();
                partyDetails = await Table.CreateItemAsync(partyDetails);
                PartyDetails partyDetailsDB = partyDetails;
                await manager.InsertItemAsync(partyDetailsDB);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Upload] Error = {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                joinButtonLabel = "Party Created!";
                RootPage.instance.GoToDetailPage(MasterDetailPageManager.MapPage);
            }
        }

        private async Task ExecuteFindLocationCommand()
        {
            var page = new FindLocationPopUpPage();

            await PopupNavigation.PushAsync(page);
        }

        private async Task ExecuteChoosePriceCommand()
        {
            var page = new PricePopupPage();
            await PopupNavigation.PushAsync(page);
        }

        private async Task SelectImageCommand()
        {
            isPartyHeaderEnabled = false;
            partyHeaderImageStream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (partyHeaderImageStream != null)
            {
                partyHeaderImageSource = ImageSource.FromStream(() => partyHeaderImageStream);
                isPartyHeaderEnabled = true;
                partyHeaderByteArray = ReadFully(partyHeaderImageStream);
                partyHeaderImageSource = ImageSource.FromStream(() => new MemoryStream(partyHeaderByteArray));
                pictureSelected = true;
            }
            else
            {
                isPartyHeaderEnabled = true;
            }
        }

        #endregion

        #region Getters & Setters
        public ImageSource partyHeaderImageSource
        {
            get { return _partyHeaderImageSource; }
            set { SetProperty(ref _partyHeaderImageSource, value, "partyHeaderImageSource"); }
        }

        public bool isPartyHeaderEnabled
        {
            get { return _isPartyHeaderEnabled; }
            set { SetProperty(ref _isPartyHeaderEnabled, value, "isPartyHeaderEnabled"); }
        }

        public string joinButtonLabel
        {
            get { return _joinButtonLabel; }
            set { SetProperty(ref _joinButtonLabel, value, "joinButtonLabel"); }
        }

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value, "SearchText"); }
        }

        public bool isJoinButtonActive
        {
            get { return _isJoinButtonActive; }
            set { SetProperty(ref _isJoinButtonActive, value, "isJoinButtonActive"); }
        }

        public PartyDetails partyDetails
        {
            get { return _partyDetails; }
            set { SetProperty(ref _partyDetails, value, "partyDetails", evaluatePartyCmd); }
        }

        string _partyDetailsTitle;
        public string partyDetailsTitle
        {
            get { return _partyDetails.title; }
            set { _partyDetails.title = value; SetProperty(ref _partyDetailsTitle, value, "partyDetailsTitle", evaluatePartyCmd); }
        }

        string _partyDetailsAdresse;
        public string partyDetailsAdresse
        {
            get { return _partyDetails.where; }
            set { _partyDetails.where = value; SetProperty(ref _partyDetailsAdresse, value, "partyDetailsAdresse", evaluatePartyCmd); }
        }

        string _partyDetailsDescription;
        public string partyDetailsDescription
        {
            get { return _partyDetails.description; }
            set { _partyDetails.description = value; SetProperty(ref _partyDetailsDescription, value, "partyDetailsDescription", evaluatePartyCmd); }
        }


        TimeSpan _partyTime;
        public TimeSpan partyTime
        {
            get { return _partyTime; }
            set { _partyTime = value; SetProperty(ref _partyTime, value, "partyTime"); ConcatenateDateTime(); }
        }

        DateTime _partyDate;
        public DateTime partyDate
        {
            get { return _partyDate; }
            set { _partyDate = value; SetProperty(ref _partyDate, value, "partyDate"); ConcatenateDateTime(); }
        }

        //DateTime _partyDateTime;
        //public DateTime partyDateTime
        //{
        //    get { return _partyDateTime; }
        //    set { _partyDateTime = value; SetProperty(ref _partyDateTime, value, "partyDateTime"); }
        //}
        #endregion

        public CreatePartyViewModel(View parent)
        {
            this.parent = (CreatePartyView)parent;
            Initialize();
        }

        public CreatePartyViewModel()
        {
            Initialize();
        }

        void Initialize()
        {
            Title = Constants.CreatePartyPageTitle;
            partyDetails = new PartyDetails()
            {
                price = 0,
                ageMin = 0,
                ageMax = 0,
                where = string.Empty,
                when = DateTime.Now,
                picture = Constants.picturePlaceholder,
                title = string.Empty,
                description = Constants.descriptionPlaceholder,
                hostpicture = App.UserDetails.picture
            };
            partyHeaderImageSource = ImageSource.FromFile(partyDetails.picture);
            joinButtonLabel = "Create Party";
            geoCoder = new Geocoder();
            positions = new List<Position>();
        }

        void ConcatenateDateTime()
        {
            PartyDetails details = partyDetails;
            details.when = partyDate.Add(partyTime);
            partyDetails = details;
            partyDetails.when = details.when;
            //partyDetails.time = partyTime;
        }

        public byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
