using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Pags
{
    public class Pages_Request_Post
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Application_Id { get; set; }
    }
}
