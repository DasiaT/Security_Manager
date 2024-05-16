using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Privilegs;
using Manager_Security_BackEnd.Models.Rols;
using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Applications_Rol_Privileges
{
    public class Application_Rol_Privileges
    {
        public int Id { get; set; }
        public int Rol_Id { get; set; }
        public int Application_Id { get; set; }
        public int Privilege_Id { get; set; }
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update { get; set; }
        public Application? Application { get; set; }
        public Roles? Roles { get; set; }
        public Privileges? Privileges { get; set; }
    }
}
