using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models;
using goParty.Models.APIModels;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Linq;
using Newtonsoft.Json;
using Stripe;

namespace goParty.Services
{
    public class AzureCloudService : ICloudService
    {
        public MobileServiceClient client;
        List<AppServiceIdentity> identities = null;
        public AzureCloudService()
        {
            client = new MobileServiceClient(Locations.AppServiceUrl, new AuthenticationDelegatingHandler());

            if (Locations.AlternateLoginHost != null)
                client.AlternateLoginHost = new Uri(Locations.AlternateLoginHost);
        }

        public ICloudTable<T> GetTable<T>() where T : TableData => new AzureCloudTable<T>(client);

        public async Task<MobileServiceUser> LoginAsync()
        {
            var loginProvider = DependencyService.Get<ILoginProvider>();
            client.CurrentUser = loginProvider.RetrieveTokenFromSecureStore();
            if (client.CurrentUser != null)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshed = await client.RefreshUserAsync();
                    if (refreshed != null)
                    {
                        loginProvider.StoreTokenInSecureStore(refreshed);
                        return refreshed;
                    }
                }
                catch (Exception refreshException)
                {
                    Debug.WriteLine($"Could not refresh token: {refreshException.Message}");
                }
            }

            if (client.CurrentUser != null && !IsTokenExpired(client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return client.CurrentUser;
            }

            // We need to ask for credentials at this point
            await loginProvider.LoginAsync(client);
            if (client.CurrentUser != null)
            {
                // We were able to successfully log in
                loginProvider.StoreTokenInSecureStore(client.CurrentUser);
            }
            return client.CurrentUser;
        }

        public async Task LogoutAsync()
        {
            if (client.CurrentUser == null || client.CurrentUser.MobileServiceAuthenticationToken == null)
                return;

            // Log out of the identity provider (if required)

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{client.MobileAppUri}/.auth/logout");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            // Remove the token from the cache
            DependencyService.Get<ILoginProvider>().RemoveTokenFromSecureStore();

            // Remove the token from the MobileServiceClient
            await client.LogoutAsync();
        }


        public async Task<AppServiceIdentity> GetIdentityAsync()
        {
            if (client.CurrentUser == null || client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            if (identities == null)
            {
                //client.CurrentUser.
                identities = await client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");
            }

            if (identities.Count > 0)
                return identities[0];
            return null;
        }

        bool IsTokenExpired(string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split(new Char[] { '.' })[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }

        public async Task RegisterForPushNotifications()
        {
            var platformProvider = DependencyService.Get<ILoginProvider>();
            await platformProvider.RegisterForPushNotifications(client);
        }

        async Task RetreiveExtraDataFromCloud()
        {
            Application app = Application.Current;

            //Get UserId from social provider
            var identity = await GetIdentityAsync();
            string userId;
            if (identity != null)
            {
                userId = client.CurrentUser.UserId;//identity.UserClaims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
                var loginProvider = DependencyService.Get<ILoginProvider>();

                try
                {
                    bool registered = await IsUserRegisteredInTheCloud(loginProvider);
                    if (!registered) //Create new user and request additional information
                    {
                        var request = new OAuth2Request("GET", new Uri(Locations.FacebookRequestUserInfoUrl), null, App.account);
                        var response = await request.GetResponseAsync();

                        UserInfo userTobeStored = JsonConvert.DeserializeObject<UserInfo>(await response.GetResponseTextAsync());
                        UserDetails tempUserDetails = new UserDetails
                        {
                            name = userTobeStored.name,
                            picture = userTobeStored.picture.data.url,
                            userId = userId,//userTobeStored.Id,
                            age = userTobeStored.age_range.min,
                            rating = 0
                        };

                        tempUserDetails = await GetTable<UserDetails>().CreateItemAsync(tempUserDetails);
                        App.userDetails = tempUserDetails;
                        Account account = loginProvider.RetreiveAccountFromSecureStore();
                        account.Properties.Add("table_id", tempUserDetails.Id);
                        loginProvider.SaveAccountInSecureStore(account);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Login] Error = {ex.Message}");
                }
            }
        }

        async Task<bool> IsUserRegisteredInTheCloud(ILoginProvider loginProvider)
        {
            ICloudTable<UserDetails> Table = GetTable<UserDetails>();
            string tableId = loginProvider.RetrieveTableIdFromSecureStore();
            Account account = loginProvider.RetreiveAccountFromSecureStore();

            if (tableId == null)
            {
                ICollection<UserDetails> tempUserDetailsList = await Table.ReadAllItemsAsync();
                UserDetails userDetails = tempUserDetailsList.FirstOrDefault(x => x.userId == client.CurrentUser.UserId);
                account.Properties.Add("table_id", userDetails.Id);
                loginProvider.SaveAccountInSecureStore(account);
                App.userDetails = userDetails;
            }
            else
            {
                UserDetails tempUserDetails = await Table.ReadItemAsync(tableId);
                App.userDetails = tempUserDetails;
            }

            if (App.userDetails != null)
                return true;
            return false;
        }

        async Task ICloudService.RetreiveExtraDataFromCloud()
        {
            await RetreiveExtraDataFromCloud();
        }

        public async Task<ICollection<PartyDetails>> RetreivePartiesWithinRange(double longitude, double latitude, double range)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("longitude", longitude.ToString());
            dictionary.Add("latitude", latitude.ToString());
            dictionary.Add("range", range.ToString());
            return await client.InvokeApiAsync<ICollection<PartyDetails>>("Party", HttpMethod.Get, dictionary);
        }

        public async Task<ICollection<PartyDetails>> RetreivePartiesWithinRange(double longitude, double latitude, double range, int type)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("longitude", longitude.ToString());
            dictionary.Add("latitude", latitude.ToString());
            dictionary.Add("range", range.ToString());
            dictionary.Add("type", type.ToString());
            return await client.InvokeApiAsync<ICollection<PartyDetails>>("Party", HttpMethod.Get, dictionary);
        }
    }
}