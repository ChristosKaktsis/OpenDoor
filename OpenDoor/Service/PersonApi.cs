using OpenDoor.Models;
using System.Text;
using System.Text.Json;

namespace OpenDoor.Service
{
    public class PersonApi :BaseApi
    {
        public static async Task<Person> GetItemAsync(string id)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync($"{Url}/{id}");
            return JsonSerializer.Deserialize<Person>(result);
        }
    }
}
