namespace Manager_Security_BackEnd.Models.Pags
{
    public class Pages_Request_Patch
    {
        public int Page_Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Application_Id { get; set; }
    }
}
