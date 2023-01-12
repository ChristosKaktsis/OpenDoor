using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenDoor.Models
{
    public class Person
    {
        [JsonPropertyName("ΚωδΚάρτας")]
        public string CodeNo { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("ΓονέαςΚηδεμόνας")]
        public string Parent { get; set; }
        [JsonPropertyName("Παρουσιολόγιο")]
        public List<Presence> Attendances { get; set; }
    }
}
