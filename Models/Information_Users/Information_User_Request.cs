using System.ComponentModel.DataAnnotations;

namespace Manager_Security_BackEnd.Models.Information_Users
{
    public class Information_User_Request
    {
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public string DNI { get; set; }
        public string? Descripcion { get; set; }
        public int Id_workstation { get; set; }
    }
}
