using goParty.Models;
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
    class AzureDocumentManager
    {
        static AzureDocumentManager defaultInstance = new AzureDocumentManager();

        const string accountURL = @"https://partycrasher.documents.azure.com:443/";
        const string accountKey = @"efOra2RFBzIaZNS3yu5wc9tVNWnDkOr9sRkqSjESt5EunzzwzWeVIjpXLOcDkHlWKL85gxptUFOjzeoRsrHL6Q==";
        const string databaseId = @"PartyDetails";
        const string collectionId = @"Party";

        private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

        private DocumentClient client;

        private AzureDocumentManager()
        {
            client = new DocumentClient(new System.Uri(accountURL), accountKey);
        }

        public static AzureDocumentManager DefaultManager
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

        public async Task<List<PartyDetails>> GetPartiesFromLocationAsync(double latt, double lon, double range)
        {
            try
            {
                // The query excludes completed TodoItems
                var query = client.CreateDocumentQuery<PartyDetails>(collectionLink, new FeedOptions { MaxItemCount = -1 })
                      .Where(party => party.location.Distance(new Point(latt, lon)) < range) //Less than one kilometer
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

        public async Task<PartyDetails> InsertItemAsync(PartyDetails todoItem)
        {
            try
            {
                var result = await client.CreateDocumentAsync(collectionLink, todoItem);
                todoItem.Id = result.Resource.Id;
                Items.Add(todoItem);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return todoItem;
        }
    }
}
