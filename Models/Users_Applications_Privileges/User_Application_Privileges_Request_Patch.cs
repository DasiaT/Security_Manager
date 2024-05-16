namespace Manager_Security_BackEnd.Models.Users_Applications_Privileges
{
    public class User_Application_Privileges_Request_Patch
    {
        public int Id { get; set; }
        public int? User_Id { get; set; }
        public int? Privileges_Id { get; set; }
        public int? Application_Id { get; set; }
        public int? Emp_Id { get; set; }
    }
}
