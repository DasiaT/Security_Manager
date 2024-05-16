using System.Text.Json.Serialization;

namespace Manager_Security_BackEnd.Models.Element_Pages
{
    public class Element_Page
    {
        public int Element_Id { get; set; }
        public string Name_Element { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public DateTime? Date_Insert { get; set; }
        [JsonIgnore]
        public DateTime? Date_Update { get; set; }
        
    }
}
