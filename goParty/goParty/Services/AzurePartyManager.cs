using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.ViewModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;
using Microsoft.Azure.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace goParty.Services
{
    class AzurePartyManager
    {
        static AzurePartyManager defaultInstance = new AzurePartyManager();

        const string accountURL = @"https://partycrasher.documents.azure.com:443/";
        const string accountKey = @"iHjB8jnyssBvYq8MR6TzoDq04EFL3oH3bNyEUfQ7i8QzCFyk2MyPzob4cosoKnJeK3M82pZnkkxn3XLDaEUm6g==";
        const string databaseId = @"PartyDetails";
        const string collectionId = @"Party";

        private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

        private DocumentClient client;

        private AzurePartyManager()
        {
            client = new DocumentClient(new System.Uri(accountURL), accountKey);
        }

        public static AzurePartyManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public List<PartyDetails> Items { get; private set; }
        public List<PartyDetailsItem> CarouselItems { get; private set; }

        public async Task<List<PartyDetails>> GetPartiesFromLocationAsync(double latt, double lon, double range)
        {
            try
            {
                // The query excludes completed TodoItems
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                ICollection<PartyDetails> Parties = await cloudService.RetreivePartiesWithinRange(lon, latt, range);
                //var query = client.CreateDocumentQuery<PartyDetailsDB>(collectionLink, new FeedOptions { MaxItemCount = -1, EnableScanInQuery = true })
                //      .Where(party => party.location.Distance(new Microsoft.Azure.Documents.Spatial.Point(lon, latt)) < range) //1 = 1m
                //      .AsDocumentQuery();
                //
                Items = new List<PartyDetails>();
                Items.AddRange(Parties.ToList());
                
                //Items.AddRange(Parties);
                //
                //while (query.HasMoreResults)
                //{
                //    Items.AddRange(await query.ExecuteNextAsync<PartyDetailsDB>());
                //}
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }
            return Items;
        }

        public async Task<List<PartyDetailsItem>> GetPartiesUserIsHostingAsync()
        {
            List<PartyDetailsItem> tempCarouselList = new List<PartyDetailsItem>();
            List<PartyDetails> tempPartyList = new List<PartyDetails>();

            //Query databse for parties user is hosting
            try
            {
                var query = client.CreateDocumentQuery<PartyDetails>(collectionLink, new FeedOptions { MaxItemCount = -1, EnableScanInQuery = true })
                     .Where(party => party.userId == App.userDetails.userId) //1 = 1m
                     .AsDocumentQuery();
                tempPartyList = new List<PartyDetails>();
                while (query.HasMoreResults)
                {
                    tempPartyList.AddRange(await query.ExecuteNextAsync<PartyDetails>());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("$[Query]" + ex.Message);
            }
            //Convert parties to CarouselParties
            foreach (var item in tempPartyList)
            {
                PartyDetailsItem tempParty = new PartyDetailsItem(item);
                tempParty.isThisUserHosting = true;
                tempParty.joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.CancelEvent];
                tempCarouselList.Add(tempParty);
            }
            return tempCarouselList;
        }

        public async Task<List<PartyDetailsItem>> GetCarouselItemsAsync(List<PartyDetails> partyList)
        {
            if (partyList == null || partyList.Count < 1)
            {
                await Application.Current.MainPage.DisplayAlert("Alert!", "No Parties Found", "OK");
                throw new NullReferenceException();
            }
            //First convert parties to CarouselParties
            List<PartyDetailsItem> tempCarouselList = new List<PartyDetailsItem>();
            foreach (var item in partyList)
            {
                PartyDetailsItem tempItem = new PartyDetailsItem(item);
                tempItem.index = tempCarouselList.Count;
                tempCarouselList.Add(tempItem);
            }

            //Get Table with attendee details
            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            ICloudTable<AttendeeDetails> Table = cloudService.GetTable<AttendeeDetails>();


            //Find out which parties the user is attending
            try
            {
                //var searchResults = await App.UserDetailsUserIdSearchIndexClient.Documents.SearchAsync<AttendeeDetails>(App.userDetails.Id);
                //ObservableRangeCollection<PartyDetails> partiesAttending = PartiesAttending;
                //ObservableRangeCollection<PartyDetails> partiesHosting = PartiesHosting;
                //
                //if (partiesAttending != null && partiesAttending.Count > 0)
                //{
                //    var myParties = from m in tempCarouselList
                //                    let fr = (from f in partiesAttending select f.partyId)
                //                    where fr.Contains(m.partyId)
                //                    select m.isThisUserAttending == true;
                //    //myParties;
                //}
                //
                //if (partiesHosting != null && partiesHosting.Count > 0)
                //{
                //    var myParties = from m in tempCarouselList
                //                    let fr = (from f in partiesHosting select f.userId)
                //                    where fr.Contains(App.userDetails.userId)
                //                    select m.isThisUserHosting = true;
                //    //myParties;
                //}

                //User found for every match make party special 
                
            }catch(Exception ex)
            {
                Console.WriteLine($"[Login] Error = {ex.Message}");
            }
            CarouselItems = tempCarouselList;
            return tempCarouselList;
        }

        public void SortCarouselPartiesToIndex(int index)
        {
            //PartyDetailsDBCarouselItem item = CarouselItems[MyMath.Clamp(index,0,CarouselItems.Count-1)];
            //CarouselItems[index] = CarouselItems[0];
            //CarouselItems[0] = item;
        }
        

        public async Task<List<PartyDetails>> GetAllPartiesAsync()
        {
            try
            {
                // The query excludes completed TodoItems
                var query = client.CreateDocumentQuery<PartyDetails>(collectionLink, new FeedOptions { MaxItemCount = -1 })
                      .AsDocumentQuery();

                Items = new List<PartyDetails>();
                while (query.HasMoreResults)
                {
                    Items.AddRange(await query.ExecuteNextAsync<PartyDetails>());
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }

            return Items;
        }

        public async Task<List<PartyDetails>> DeleteAllBuggedPartiesAsync()
        {
            try
            {
                // The query excludes completed TodoItems
                SqlQuerySpec specs = new SqlQuerySpec();
                var query = client.CreateDocumentQuery<PartyDetails>(collectionLink, new FeedOptions { MaxItemCount = -1 })
                      .Where(party => party.picture == null)
                      .AsDocumentQuery();

                List<PartyDetails> deleteItems = new List<PartyDetails>();
                while (query.HasMoreResults)
                {
                    //await Task.WhenAll(Items)
                    deleteItems.AddRange(await query.ExecuteNextAsync<PartyDetails>());
                }
                await Task.WhenAll(deleteItems.Select(x => DeleteItemAsync(x)));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }
            return Items;
        }

        public ICloudTable<PartyDetails> Table { get; set; }

        public async Task DeleteItemAsync(PartyDetails partyDetailsDB)
        {
            try
            {
                var result = await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, partyDetailsDB.Id));

                if(Items != null && Items.Contains(partyDetailsDB))
                    Items.Remove(partyDetailsDB);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }

        public async Task<PartyDetails> InsertItemAsync(PartyDetails partyDetailsDB)
        {
            try
            {
                //First insert party in document Db
                var result = await client.CreateDocumentAsync(collectionLink, partyDetailsDB);
                partyDetailsDB.Id = result.Resource.Id;
                if (Items == null)
                    Items = new List<PartyDetails>();
                Items.Add(partyDetailsDB);
                //Then insert party in sql table
               
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return partyDetailsDB;
        }


        public static ObservableRangeCollection<PartyDetails> PartiesAttending { get; set; }
        public static ObservableRangeCollection<PartyDetails> PartiesHosting { get; set; }

        public async void QueryForPartiesAttending()
        {
            ICloudService cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            ICloudTable<PartyDetails> Table = cloudService.GetTable<PartyDetails>();
            ICollection<PartyDetails> attendeeDetails = await Table.ReadAllItemsAsync();
            List<PartyDetailsItem> carouselItems = new List<PartyDetailsItem>();
            List<Image> carouselImages = new List<Image>();
            if (attendeeDetails == null || attendeeDetails.Count < 0)
                return;

            ICollection<PartyDetails> partiesAttending = new List<PartyDetails>();
            ICollection<PartyDetails> partiesHosting = new List<PartyDetails>();

            foreach (var item in attendeeDetails)
            {
                if (item.userId == App.userDetails.userId)
                {
                    partiesHosting.Add(item);
                }
                else
                {
                    partiesAttending.Add(item);
                }
            }
           
            ImageHelper.LoadedImages.Clear();
            PartiesAttending.AddRange(partiesAttending);
            PartiesHosting.AddRange(partiesHosting);
        }


        public async Task<ImageSource> LoadImage(string picture)
        {
            if (picture.Length > 10)
            {
                ImageHelper.ImageHelperItem item = ImageHelper.LoadedImages.FirstOrDefault(x => x.imageId == picture);
                ImageSource img = item?.image;
                return img ?? await AzureStorage.LoadImage(picture);
            }
            else
            {
                ImageHelper.ImageHelperItem item = ImageHelper.LoadedImages.FirstOrDefault(x => x.imageId == picture);
                ImageSource img;
                if (item == null)
                {
                    img = ImageSource.FromFile(picture);
                    ImageHelper.LoadedImages.Add(new ImageHelper.ImageHelperItem { image = img, imageId = picture });
                    return img;
                }
                return item.image;
            }
        }
    }
}
