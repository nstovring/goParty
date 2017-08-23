using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.MobileServices;
using goParty.Abstractions;
using goParty.Droid.Services;
using goParty.Helpers;
using Xamarin.Auth;
using Newtonsoft.Json.Linq;
using Gcm.Client;
using System.Diagnostics;
using System.Net.Http;
using System.Collections.Generic;
using Android.Util;
using Newtonsoft.Json;
//using Xamarin.Facebook.Login;
//using Xamarin.Forms.Platform.Android;
//using Xamarin.Facebook;

[assembly: Xamarin.Forms.Dependency(typeof(DroidLoginProvider))]
namespace goParty.Droid.Services
{
    public class DroidLoginProvider : ILoginProvider
    {
        #region ILoginProvider Interface
        public MobileServiceUser RetrieveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService(Locations.AppName);
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    string token;

                    if (acct.Properties.TryGetValue("token", out token))
                    {
                        return new MobileServiceUser(acct.Username)
                        {
                            MobileServiceAuthenticationToken = token
                        };
                    }
                }
            }
            return null;
        }

        public string RetrieveTableIdFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService(Locations.AppName);
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    string tableId;

                    if (acct.Properties.TryGetValue("table_id", out tableId))
                    {
                        return tableId;
                    }
                }
            }
            return null;
        }

        public Account RetreiveAccountFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService(Locations.AppName);
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    return acct;
                }
            }
            return null;
        }

        public void SaveAccountInSecureStore(Account account)
        {
            AccountStore.Save(account, Locations.AppName);
        }

        public void StoreTokenInSecureStore(MobileServiceUser user)
        {
            var account = new Account(user.UserId);
            account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            AccountStore.Save(account, Locations.AppName);
        }

        public void RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService(Locations.AppName);
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    AccountStore.Delete(acct, Locations.AppName);
                }
            }
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        {
            //#region Azure AD Client Flow
            // var accessToken = await LoginADALAsync();
            //var zumoPayload = new JObject();
            //zumoPayload["access_token"] = accessToken;
            //return await client.LoginAsync("facebook", zumoPayload);
            //#endregion

            #region Auth0 Client Flow
            // var accessToken = await LoginAuth0Async();
            //var zumoPayload = new JObject();
            //zumoPayload["access_token"] = accessToken;
            //return await client.LoginAsync("auth0", zumoPayload);
            #endregion

            //Server Flow
            //return await client.LoginAsync(RootView, "facebook", "baldrbytesgoparty");

            //Native facebook
            return await LoginFacebookAsync(client);

            //return await client.LoginAsync(RootView, "facebook", Locations.FacebookRedirectUri);
            //return client.CurrentUser;
        }

        public async Task RegisterForPushNotifications(MobileServiceClient client)
        {
            if (GcmClient.IsRegistered(RootView))
            {
                try
                {
                    var registrationId = GcmClient.GetRegistrationId(RootView);
                    //var push = client.GetPush();
                    //await push.RegisterAsync(registrationId);

                    var installation = new DeviceInstallation
                    {
                        InstallationId = client.InstallationId,
                        Platform = "gcm",
                        PushChannel = registrationId
                    };
                    // Set up tags to request
                    installation.Tags.Add("topic:NearbyEvents");
                    // Set up templates to request
                    PushTemplate genericTemplate = new PushTemplate
                    {
                        Body = @"{""data"":{""message"":""$(message)""}}"
                    };
                    installation.Templates.Add("genericTemplate", genericTemplate);

                    // Register with NH
                    var response = await client.InvokeApiAsync<DeviceInstallation, DeviceInstallation>(
                        $"/push/installations/{client.InstallationId}",
                        installation,
                        System.Net.Http.HttpMethod.Put,
                        new Dictionary<string, string>());
                }
                catch (Exception ex)
                {
                    Log.Error("DroidPlatformProvider", $"Could not register with NH: {ex.Message}");
                }
            }
            else
            {
                Log.Error("DroidPlatformProvider", $"Not registered with FCM");
            }
        }

        #endregion


        public Context RootView { get; private set; }

        public AccountStore AccountStore { get; private set; }

        public void Init(Context context)
        {
            RootView = context;
            AccountStore = AccountStore.Create(context);

            try
            {
                // Check to see if this client has the right permissions
                GcmClient.CheckDevice(RootView);
                GcmClient.CheckManifest(RootView);

                // Register for push
                GcmClient.Register(RootView, GcmHandler.SenderId);
                Debug.WriteLine($"GcmClient: Registered for push with FCM: {GcmClient.GetRegistrationId(RootView)}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GcmClient: Cannot register for push: {ex.Message}");
            }

        }

        #region Azure AD Client Flow
        public async Task<string> LoginADALAsync()
        {
            Uri returnUri = new Uri(Helpers.Constants.FacebookRedirectUri);

            var authContext = new AuthenticationContext(Locations.AadAuthority);
            if (authContext.TokenCache.ReadItems().Count() > 0)
            {
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
            }
            var authResult = await authContext.AcquireTokenAsync(
                Locations.AppServiceUrl, /* The resource we want to access  */
                Locations.AadClientId,   /* The Client ID of the Native App */
                returnUri,               /* The Return URI we configured    */
                new PlatformParameters((Activity)RootView));
            return authResult.AccessToken;
        }
        #endregion

        #region Facebook Client Flow

        public async Task<MobileServiceUser> LoginFacebookAsync(MobileServiceClient client)
        {
            var authenticator = new OAuth2Authenticator(
                 Helpers.Constants.FacebookClientId,
                 Helpers.Constants.FacebookRequestScope,
                 new Uri("https://m.facebook.com/dialog/oauth/"),
                 new Uri("https://www.facebook.com/connect/login_success.html")
                 )
            { Title = "Login with Facebook to acess goParty",
            AllowCancel = true
            };

            var tcs = new TaskCompletionSource<MobileServiceUser>();

            authenticator.Completed += async (sender, e) => {
                Account acc = e.Account;
                App.account = acc;
                string accessToken = acc.Properties["access_token"];
                var zumoPayload = new JObject();
                zumoPayload.Add("access_token", accessToken);
                //zumoPayload["access_token"] = accessToken;

                client.CurrentUser = new MobileServiceUser(Locations.AppServiceUrl)
                { MobileServiceAuthenticationToken = accessToken };

                //Send the token to Azure App Service to auth with backend 
                //return await client.LoginAsync()
                tcs.SetResult(await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, zumoPayload));
            };

            authenticator.Error += (sender, e) => tcs.SetException(e.Exception);

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
            //// iOS
            //var ui = auth.GetUI();
            //someViewController.PresentViewController(ui, true, null);
            return await tcs.Task; 
        }


        #endregion

        #region Auth0 Client Flow
        //public async Task<string> LoginAuth0Async()
        //{
        //    var auth0 = new Auth0.SDK.Auth0Client("shellmonger.auth0.com", "lmFp5jXnwPpD9lQIYwgwwPmFeofuLpYq");
        //    var user = await auth0.LoginAsync(RootView, scope: "openid email name");
        //    return user.IdToken;
        //}
        #endregion


        AccountStore ILoginProvider.AccountStore()
        {
            return AccountStore;
        }
    }
}