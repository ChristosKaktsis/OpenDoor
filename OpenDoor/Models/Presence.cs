using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenDoor.Models
{
    public class Presence
    {
        [JsonPropertyName("oid")]
        public string Oid { get; set; }
        [JsonPropertyName("startOn")]
        public DateTime StartOn { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
    }
}
