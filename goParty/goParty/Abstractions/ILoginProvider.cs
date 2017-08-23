using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace goParty.Abstractions
{
    public interface ILoginProvider
    {
        MobileServiceUser RetrieveTokenFromSecureStore();

        void StoreTokenInSecureStore(MobileServiceUser user);

        void RemoveTokenFromSecureStore();

        Task<MobileServiceUser> LoginAsync(MobileServiceClient client);

        Task RegisterForPushNotifications(MobileServiceClient client);
        Account RetreiveAccountFromSecureStore();
        string RetrieveTableIdFromSecureStore();
        void SaveAccountInSecureStore(Account account);
        AccountStore AccountStore();
    }
}
