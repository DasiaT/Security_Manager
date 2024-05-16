namespace Manager_Security_BackEnd.Models.Companys
{
    public class Company_Request_Patch
    {
        public int Emp_Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
