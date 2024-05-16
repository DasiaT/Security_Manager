using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Rols
{
    public class Roles
    {
        public int Rol_Id { get; set; }
        public string Name { get; set;}
        public string? Description { get; set;}
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update { get; set; }
    }
}
