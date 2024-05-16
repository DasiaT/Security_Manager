using Manager_Security_BackEnd.Models.Applications;
using System.Text.Json.Serialization;
using UserType = Manager_Security_BackEnd.Models.Users.User;

namespace Manager_Security_BackEnd.Models.Authentications
{
    public class Authentication
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int User_Id { get; set; }
        public int Application_Id { get; set; }
        public Application? Application { get; set; }
        public UserType? User { get; set; }
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Session { get; set; }
        public DateTime? Date_Expires_Token { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update_Token { get; set; }
    }
}
