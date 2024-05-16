using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Companys;
using Manager_Security_BackEnd.Models.Rols;
using System.Text.Json.Serialization;
using UserType = Manager_Security_BackEnd.Models.Users.User;

namespace Manager_Security_BackEnd.Models.Users_Applications_Rols
{
    public class User_Application_Rol
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Rol_Id { get; set; }
        public int Application_Id { get; set; }
        public int Emp_Id { get; set; }

        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update { get; set; }
        public UserType? User { get; set; } 
        public Roles? Roles { get; set; }
        public Application? Application { get; set; }
        public Company? Company { get; set; }
    }
}
