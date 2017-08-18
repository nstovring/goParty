using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Helpers;
using Xamarin.Auth;
using Microsoft.Azure.Search;
using goParty.Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace goParty.ViewModels
{
    public class EntryPageViewModel : BaseViewModel
    {
        public EntryPageViewModel()
        {
            Title = "Task List";
        }

        Command loginCmd;
        public Command LoginCommand => loginCmd ?? (loginCmd = new Command(async () => await ExecuteLoginCommand().ConfigureAwait(false)));

        async Task ExecuteLoginCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                await cloudService.LoginAsync();
                await RetreiveExtraDataFromCloud(cloudService);
                await cloudService.RegisterForPushNotifications();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }
            finally
            {
                Application.Current.MainPage = new NavigationPage(new Pages.RootPage());
                IsBusy = false;
            }
        }

        async Task RetreiveExtraDataFromCloud(ICloudService cloudService)
        {
            Application app = Application.Current;
            
            //Get UserId from social provider
            var identity = await cloudService.GetIdentityAsync();
            string userId;
            if (identity != null)
            {
                userId = identity.UserClaims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;


                if(App.UserIdSearchIndexClient == null)
                    App.UserIdSearchIndexClient = new SearchIndexClient(Constants.SearchServiceName, Constants.userIDIndex, new SearchCredentials(Constants.SearchAdminApiKey));

                try
                {
                    var searchResults = await App.UserIdSearchIndexClient.Documents.SearchAsync<UserDetails>(userId);
                
                    //Assign found user to app
                    if (searchResults.Results.Count > 0)
                    {
                        UserDetails tempUserDetails = new UserDetails{
                        name = searchResults.Results[0].Document.name,
                        picture = searchResults.Results[0].Document.picture,
                        userId = searchResults.Results[0].Document.userId,
                        age = searchResults.Results[0].Document.age,
                        rating = searchResults.Results[0].Document.rating
                        };

                        App.userDetails = tempUserDetails;
                    }
                    else //Create new user and request additional information
                    {
                        var request = new OAuth2Request("GET", new Uri(Locations.FacebookRequestUserInfoUrl), null, App.account);

                        var response = await request.GetResponseAsync();

                        UserInfo userTobeStored = JsonConvert.DeserializeObject<UserInfo>(await response.GetResponseTextAsync());
                        UserDetails tempUserDetails = new UserDetails
                        {
                            name = userTobeStored.name,
                            picture = userTobeStored.picture.data.url,
                            userId = userTobeStored.Id,
                            age = userTobeStored.age_range.min,
                            rating = 0
                        };

                        App.userDetails = tempUserDetails;
                        await cloudService.GetTable<UserDetails>().CreateItemAsync(tempUserDetails); 
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Login] Error = {ex.Message}");
                }
            }
            //var list = await Table.ReadAllItemsAsync();
            //Items.ReplaceRange(list);
            //Query table for userID
        }


        //async Task<UserDetails> CreateUserDetailsFromFacebookAccount(Account facebookAccount)
        //{
        //    var request = new OAuth2Request("GET", new Uri(Locations.FacebookRequestUserInfoUrl), null, facebookAccount);
        //
        //    FacebookUserDetails facebookUserToBeStored = new FacebookUserDetails();
        //
        //    try
        //    {
        //        var response = await request.GetResponseAsync();
        //        if (response != null)
        //        {
        //            string userJson = response.GetResponseText();
        //            facebookUserToBeStored = JsonConvert.DeserializeObject<FacebookUserDetails>(userJson);
        //        }
        //
        //        if (facebookUserToBeStored != null)
        //        {
        //            facebookUserToBeStored.userId = facebookUserToBeStored.Id;
        //            facebookUserToBeStored.Id = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Data.ToString());
        //        Console.WriteLine("Well this is bad");
        //    }
        //
        //    return ConvertDetails(facebookUserToBeStored);
        //}
    }
}
