﻿using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
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

        public List<PartyDetailsDB> Items { get; private set; }
        public List<PartyDetailsDBCarouselItem> CarouselItems { get; private set; }

        public async Task<List<PartyDetailsDB>> GetPartiesFromLocationAsync(double latt, double lon, double range)
        {
            try
            {
                // The query excludes completed TodoItems
                var query = client.CreateDocumentQuery<PartyDetailsDB>(collectionLink, new FeedOptions { MaxItemCount = -1, EnableScanInQuery = true })
                      .Where(party => party.location.Distance(new Point(lon, latt)) < range) //1 = 1m
                      .AsDocumentQuery();

                Items = new List<PartyDetailsDB>();
                while (query.HasMoreResults)
                {
                    Items.AddRange(await query.ExecuteNextAsync<PartyDetailsDB>());
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }
            return Items;
        }

        public async Task<List<PartyDetailsDBCarouselItem>> GetPartiesUserIsHostingAsync()
        {
            List<PartyDetailsDBCarouselItem> tempCarouselList = new List<PartyDetailsDBCarouselItem>();
            List<PartyDetailsDB> tempPartyList = new List<PartyDetailsDB>();

            //Query databse for parties user is hosting
            try
            {
                var query = client.CreateDocumentQuery<PartyDetailsDB>(collectionLink, new FeedOptions { MaxItemCount = -1, EnableScanInQuery = true })
                     .Where(party => party.userId == App.userDetails.userId) //1 = 1m
                     .AsDocumentQuery();
                tempPartyList = new List<PartyDetailsDB>();
                while (query.HasMoreResults)
                {
                    tempPartyList.AddRange(await query.ExecuteNextAsync<PartyDetailsDB>());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("$[Query]" + ex.Message);
            }
            //Convert parties to CarouselParties
            foreach (var item in tempPartyList)
            {
                PartyDetailsDBCarouselItem tempParty = new PartyDetailsDBCarouselItem(item);
                tempParty.isThisUserHosting = true;
                tempParty.joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.CancelEvent];
                tempCarouselList.Add(tempParty);
            }
            return tempCarouselList;
        }

        public async Task<List<PartyDetailsDBCarouselItem>> GetCarouselItemsAsync(List<PartyDetailsDB> partyList)
        {
            //First convert parties to CarouselParties
            List<PartyDetailsDBCarouselItem> tempCarouselList = new List<PartyDetailsDBCarouselItem>();
            foreach (var item in partyList)
            {
                tempCarouselList.Add(new PartyDetailsDBCarouselItem(item));
            }

            //Get Table with attendee details
            var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            ICloudTable<AttendeeDetails> Table = cloudService.GetTable<AttendeeDetails>();

            if (App.AttendeeUserIdSearchIndexClient == null)
                App.AttendeeUserIdSearchIndexClient = new SearchIndexClient(Constants.SearchServiceName, Constants.attendeeUserIDIndex, new SearchCredentials(Constants.SearchAdminApiKey));

            //Find out which parties the user is attending
            try
            {
                var searchResults = await App.UserDetailsUserIdSearchIndexClient.Documents.SearchAsync<AttendeeDetails>(App.userDetails.Id);

                //User found for every match make party special 
                if (searchResults.Results.Count > 0)
                {
                    foreach (var attendeeDetail in searchResults.Results)
                    {
                        foreach (var party in tempCarouselList)
                        {
                            if(attendeeDetail.Document.partyId == party.partyId)
                            {
                                party.isThisUserAttending = true;
                                party.joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.RequestSent];
                            }
                            else if(party.userId == App.userDetails.userId) {
                                party.isThisUserHosting = true;
                                party.joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.CancelEvent];
                            }
                            else
                            {
                                party.joinButtonLabel = Constants.joinButtonTitles[(int)Constants.JoinedPartyStates.JoinParty];
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"[Login] Error = {ex.Message}");
            }
            CarouselItems = tempCarouselList;
            return tempCarouselList;
        }

        public async Task<List<PartyDetailsDB>> GetAllPartiesAsync()
        {
            try
            {
                // The query excludes completed TodoItems
                var query = client.CreateDocumentQuery<PartyDetailsDB>(collectionLink, new FeedOptions { MaxItemCount = -1 })
                      .AsDocumentQuery();

                Items = new List<PartyDetailsDB>();
                while (query.HasMoreResults)
                {
                    Items.AddRange(await query.ExecuteNextAsync<PartyDetailsDB>());
                }


            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }

            return Items;
        }

        public async Task<List<PartyDetailsDB>> DeleteAllBuggedPartiesAsync()
        {
            try
            {
                // The query excludes completed TodoItems
                SqlQuerySpec specs = new SqlQuerySpec();
                var query = client.CreateDocumentQuery<PartyDetailsDB>(collectionLink, new FeedOptions { MaxItemCount = -1 })
                      .Where(party => party.picture == null)
                      .AsDocumentQuery();

                List<PartyDetailsDB> deleteItems = new List<PartyDetailsDB>();
                while (query.HasMoreResults)
                {
                    //await Task.WhenAll(Items)
                    deleteItems.AddRange(await query.ExecuteNextAsync<PartyDetailsDB>());
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


        public async Task DeleteItemAsync(PartyDetailsDB partyDetailsDB)
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

        public async Task<PartyDetailsDB> InsertItemAsync(PartyDetailsDB partyDetailsDB)
        {
            try
            {
                //First insert party in document Db
                var result = await client.CreateDocumentAsync(collectionLink, partyDetailsDB);
                partyDetailsDB.Id = result.Resource.Id;
                if (Items == null)
                    Items = new List<PartyDetailsDB>();
                Items.Add(partyDetailsDB);
                //Then insert party in sql table
               
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return partyDetailsDB;
        }
    }
}
