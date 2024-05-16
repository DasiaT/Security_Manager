using Manager_Security_BackEnd.Models.Applications;
using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Pags
{
    public class Pages
    {
        public int Page_Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update { get; set; }
        public int Application_Id { get; set; }
        public Application? Application { get; set; }
    }
}
