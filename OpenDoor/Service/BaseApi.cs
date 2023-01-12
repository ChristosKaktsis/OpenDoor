using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.Service
{
    public class BaseApi
    {
        protected static readonly string BaseAddress = "http://192.168.3.20:7248";
        protected static readonly string Url = $"{BaseAddress}/api/Person";
        private static HttpClient client;

        protected static async Task<HttpClient> GetClient()
        {
            if (client != null)
                return client;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }
    }
}
