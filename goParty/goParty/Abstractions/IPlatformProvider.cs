﻿using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace goParty.Abstractions
{
    public interface IPlatformProvider
    {
        MobileServiceUser RetrieveTokenFromSecureStore();

        void StoreTokenInSecureStore(MobileServiceUser user);

        void RemoveTokenFromSecureStore();

        Task<MobileServiceUser> LoginAsync(MobileServiceClient client);

        Task RegisterForPushNotifications(MobileServiceClient client);
    }
}
