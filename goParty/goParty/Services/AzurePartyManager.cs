using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;
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
                partyDetailsDB.documentDBId = partyDetailsDB.Id;
                var result = await client.CreateDocumentAsync(collectionLink, partyDetailsDB);
                partyDetailsDB.Id = result.Resource.Id;
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
