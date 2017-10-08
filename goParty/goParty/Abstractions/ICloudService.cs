using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using goParty.Models;
using System.Collections.Generic;

namespace goParty.Abstractions
{
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;

        Task<MobileServiceUser> LoginAsync();

        Task LogoutAsync();

        //Task LoginAsync(User user);

        Task<AppServiceIdentity> GetIdentityAsync();

        Task RegisterForPushNotifications();

        Task RetreiveExtraDataFromCloud();

        Task<ICollection<PartyDetails>> RetreivePartiesWithinRange(double longitude, double latitude, double range);
        Task<ICollection<PartyDetails>> RetreivePartiesWithinRange(double longitude, double latitude, double range, int type);

    }
}
