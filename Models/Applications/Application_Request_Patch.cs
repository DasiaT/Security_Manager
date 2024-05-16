namespace Manager_Security_BackEnd.Models.Applications
{
    public class Application_Request_Patch
    {
        public int Application_Id { get; set; }
        public string? Application_Name { get; set; }
        public string? URL_Server { get; set; }
        public string? Description { get; set; }
        public int Emp_Id { get; set; }
    }
}
