using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.Service
{
    public interface IRestable<T>
    {
        Task<T> GetItemAsync(string id);
        Task<List<T>> GetItemsAsync();
        Task<bool> UpdateItemAsync(T item);
    }
}
