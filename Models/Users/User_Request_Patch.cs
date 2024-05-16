using Manager_Security_BackEnd.Models.User;
using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Users
{
    public class User_Request_Patch
    {
        public int Id { get; set; }
        public string? User_Name { get; set; }
        public string? Password { get; set; }
        public Boolean? State { get; set; }
        public int? User_Id { get; set; }
        
    }
}
