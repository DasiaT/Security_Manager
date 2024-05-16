namespace Manager_Security_BackEnd.Models.Users_Applications_Rols
{
    public class User_Application_Rol_Request_Post
    {
        public int User_Id { get; set; }
        public int Rol_Id { get; set; }
        public int Application_Id { get; set; }
        public int Emp_Id { get; set; }
    }
}
