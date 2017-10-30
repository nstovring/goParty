using goParty.Abstractions;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goParty.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
    {
        MobileServiceClient client;
        private IMobileServiceTable<T> table;
        public IMobileServiceTable<T> GetTable() {
            return table;
        }

        public AzureCloudTable(MobileServiceClient client)
        {
            this.client = client;
            this.table = client.GetTable<T>();
        }

        #region ICloudTable implementation
        public async Task<T> CreateItemAsync(T item)
        {
            await table.InsertAsync(item);
            return item;
        }


        //public async Task<List<T>> CreateItemsAsync(List<T> items)
        //{
        //    await  Task.WhenAll(items.Select(q => table.InsertAsync(q)));
        //    return items;
        //}

        public async Task DeleteItemAsync(T item)
        {
            await table.DeleteAsync(item);
        }

        public async Task<ICollection<T>> ReadAllItemsAsync(string query)
        {
            try
            {
                return (ICollection<T>) await table.ReadAsync<T>(query);
            }catch(Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }
            return null;
        }


        public async Task<ICollection<T>> ReadAllItemsAsync()
        {
            try
            {
                return await table.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }
            return null;
        }


        //public async Task<ICollection<T>> ReadAllItemsWithinRangeAsync(double lon, double latt, double range)
        //{
        //    try
        //    {
        //        return await table.Where(;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"[Login] Error = {ex.Message}");
        //    }
        //    return null;
        //}

        public async Task<T> ReadItemAsync(string id)
        {
            return await table.LookupAsync(id);
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            await table.UpdateAsync(item);
            return item;
        }
        #endregion
    }
}
