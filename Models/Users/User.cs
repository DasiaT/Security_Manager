using Manager_Security_BackEnd.Models.User;
using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public Boolean State { get; set; }
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime Date_Update { get; set; }
        public int User_Id { get; set; }
        public Information_User? Information_User { get; set; }
    }
}
