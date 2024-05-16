using System.ComponentModel.DataAnnotations;

namespace Manager_Security_BackEnd.Models.Information_Users
{
    public class Information_User_Request_Patch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public Boolean? State { get; set; } = true;
        public string DNI { get; set; }
        public string? Descripcion { get; set; }
        public int Id_workstation { get; set; }
    }
}
