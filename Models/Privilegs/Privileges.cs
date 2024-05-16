using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Privilegs
{
    public class Privileges
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update { get; set; }
    }
}
