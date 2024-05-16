namespace Manager_Security_BackEnd.Models.Applications_Rol_Privileges
{
    public class Application_Rol_Privileges_Request_Patch
    {
        public int Id { get; set; }
        public int? Rol_Id { get; set; }
        public int? Application_Id { get; set; }
        public int? Privilege_Id { get; set; }
    }
}
