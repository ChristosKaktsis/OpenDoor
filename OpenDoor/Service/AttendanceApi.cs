using OpenDoor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.Service
{
    public  class AttendanceApi : BaseApi
    {
        public static async Task<bool> UpdateItemAsync(Presence item)
        {
            HttpRequestMessage msg = new(HttpMethod.Put, $"{Url}/Attendance/{item.Oid}");
            msg.Content = JsonContent.Create<Presence>(item);
            HttpClient client = await GetClient();
            var response = await client.SendAsync(msg);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
