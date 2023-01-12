using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenDoor.Models
{
    public class Responsibility
    {
        [JsonPropertyName("Ημνία")]
        public DateTime Date { get; set; }
        [JsonPropertyName("Αιτιολογία")]
        public string? Description { get; set; }
        [JsonPropertyName("Οφειλή")]
        public double? Price { get; set; }
    }
}
