namespace Manager_Security_BackEnd.Models.Rols
{
    public class Roles_Request_Patch
    {
        public int Rol_Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
