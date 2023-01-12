using OpenDoor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenDoor.Service
{
    public class ResponsibilityApi : BaseApi
    {
        /// <summary>
        /// Gets the responsibilities with Person code
        /// </summary>
        /// <param name="id">Person code</param>
        /// <returns>list of responsibilities of the person</returns>
        public static async Task<List<Responsibility>> GetItemsAsync(string id)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync($"{Url}/Responsibility/{id}");
            return JsonSerializer.Deserialize<List<Responsibility>>(result);
        }
    }
}
