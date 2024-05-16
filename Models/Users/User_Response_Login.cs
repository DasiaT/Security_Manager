using Manager_Security_BackEnd.Models.User;
using Manager_Security_BackEnd.Models.Users_Applications_Rols;
using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Users
{
    public class User_Login
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public int User_Id { get; set; }
        public Information_User? Information_User { get; set; }
        public Boolean State { get; set; }
        public string? Token { get; set; }
        public int? ExpiresInMinutes { get; set; }
        public User_Application_Rol? User_Application_Rol { get; set; }

    }
    public class User_Response_Login
    {
        public List<User_Login>? Result { get; set; }
        public int? Count { get; set; }
    }
}
