using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace goParty.Abstractions
{
    public interface ICloudTable<T> where T : TableData
    {
        Task<T> CreateItemAsync(T item);
        Task<T> ReadItemAsync(string id);
        Task<T> UpdateItemAsync(T item);
        Task DeleteItemAsync(T item);
        IMobileServiceTable<T> GetTable();
        Task<ICollection<T>> ReadAllItemsAsync();
        Task<ICollection<T>> ReadAllItemsAsync(string query);

    }
}
