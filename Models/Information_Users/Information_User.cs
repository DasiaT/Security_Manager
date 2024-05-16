using Manager_Security_BackEnd.Models.Workstation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.User
{
    public class Information_User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public string DNI { get; set; }
        public string? Description { get; set; }
        public Boolean State { get; set; }
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime Date_Update { get; set; }
        public int Id_workstation { get; set; }
        public Information_Workstation? Information_Workstation { get; set; }
    }
}
