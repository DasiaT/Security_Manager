using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Users
{
    public class User_Request_Post
    {
        public string User_Name { get; set; }
        public string Password { get; set; }
        public int User_Id { get; set; }
    }
}
