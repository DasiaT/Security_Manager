namespace Manager_Security_BackEnd.Models.Access_Page_Rols
{
    public class Access_Page_Rol_Request_Patch
    {
        public int Id { get; set; }
        public int Application_Id { get; set; }
        public int Rol_Id { get; set; }
        public int Page_Id { get; set; }
    }
}
