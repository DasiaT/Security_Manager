using Manager_Security_BackEnd.Models.Companys;
using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Applications
{
    public class Application
    {
        public int Application_Id { get; set; }
        public string Application_Name { get; set; }
        public string URL_Server { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update { get; set; }
        public int Emp_Id { get; set; }
        public Company? Company { get; set; }
    }
}
