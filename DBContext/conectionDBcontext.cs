
using Manager_Security_BackEnd.Models.Access_Page_Rols;
using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Applications_Rol_Privileges;
using Manager_Security_BackEnd.Models.Authentications;
using Manager_Security_BackEnd.Models.Companys;
using Manager_Security_BackEnd.Models.Element_Pages;
using Manager_Security_BackEnd.Models.Pags;
using Manager_Security_BackEnd.Models.Privilegs;
using Manager_Security_BackEnd.Models.Rols;
using Manager_Security_BackEnd.Models.User;
using Manager_Security_BackEnd.Models.Users;
using Manager_Security_BackEnd.Models.Users_Applications_Privileges;
using Manager_Security_BackEnd.Models.Users_Applications_Rols;
using Manager_Security_BackEnd.Models.Workstation;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.DBContext
{
    public class conectionDBcontext : DbContext
    {
        public conectionDBcontext(DbContextOptions<conectionDBcontext> options) : base(options) { }
        public DbSet<Access_Page_Rol> Access_Page_Rol { get; set; }
        public DbSet<Application> Application { get; set; }
        public DbSet<Application_Rol_Privileges> Application_Rol_Privileges { get; set; }
        public DbSet<Authentication> Authentication { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Element_Page> Element_Page { get; set; }
        public DbSet<Information_Workstation> Information_Workstation { get; set; }
        public DbSet<Information_User> Information_User { get; set; }
        public DbSet<Pages> Pages { get; set; }
        public DbSet<Privileges> Privileges { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<User_Application_Rol> User_Application_Rol { get; set; }
        public DbSet<User_Application_Privileges> User_Application_Privileges { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Access_Page_Rol
            modelBuilder.Entity<Access_Page_Rol>().HasKey(x => x.Id);
            modelBuilder.Entity<Access_Page_Rol>().HasOne(u => u.Application).WithMany().HasForeignKey(u => u.Application_Id);
            modelBuilder.Entity<Access_Page_Rol>().HasOne(u => u.Pages).WithMany().HasForeignKey(u => u.Page_Id);
            modelBuilder.Entity<Access_Page_Rol>().HasOne(u => u.Roles).WithMany().HasForeignKey(u => u.Rol_Id);
            //Application
            modelBuilder.Entity<Application>().HasKey(u => u.Application_Id);
            modelBuilder.Entity<Application>().HasOne(u => u.Company).WithMany().HasForeignKey(u => u.Emp_Id);
            //Application_Rol_Privileges
            modelBuilder.Entity<Application_Rol_Privileges>().HasKey(x => x.Id);
            modelBuilder.Entity<Application_Rol_Privileges>().HasOne(u => u.Application).WithMany().HasForeignKey(u => u.Application_Id);
            modelBuilder.Entity<Application_Rol_Privileges>().HasOne(u => u.Privileges).WithMany().HasForeignKey(u => u.Privilege_Id);
            modelBuilder.Entity<Application_Rol_Privileges>().HasOne(u => u.Roles).WithMany().HasForeignKey(u => u.Rol_Id);
            //Authentication
            modelBuilder.Entity<Authentication>().HasKey(u => u.Id);
            modelBuilder.Entity<Authentication>().HasOne(u => u.User).WithMany().HasForeignKey(u => u.User_Id);
            modelBuilder.Entity<Authentication>().HasOne(u => u.Application).WithMany().HasForeignKey(u => u.Application_Id);
            //Company
            modelBuilder.Entity<Company>().HasKey(u => u.Emp_Id);
            //Element_Page
            modelBuilder.Entity<Element_Page>().HasKey(u => u.Element_Id);
            //Information_Workstation
            modelBuilder.Entity<Information_Workstation>().HasKey(u => u.Id);
            //Information_User
            modelBuilder.Entity<Information_User>().HasOne(u => u.Information_Workstation).WithMany().HasForeignKey(u => u.Id_workstation);
            //Pages
            modelBuilder.Entity<Pages>().HasKey(u => u.Page_Id);
            modelBuilder.Entity<Pages>().HasOne(u => u.Application).WithMany().HasForeignKey(u => u.Application_Id);
            //Roles
            modelBuilder.Entity<Roles>().HasKey(u => u.Rol_Id);
            //Privileges
            modelBuilder.Entity<Privileges>().HasKey(u => u.Id);
            //User
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasOne(u => u.Information_User).WithMany().HasForeignKey(u => u.User_Id);
            //User_Application_Privileges
            modelBuilder.Entity<User_Application_Privileges>().HasKey(u => u.Id);
            modelBuilder.Entity<User_Application_Privileges>().HasOne(u => u.User).WithMany().HasForeignKey(u => u.User_Id);
            modelBuilder.Entity<User_Application_Privileges>().HasOne(u => u.Privileges).WithMany().HasForeignKey(u => u.Privileges_Id);
            modelBuilder.Entity<User_Application_Privileges>().HasOne(u => u.Application).WithMany().HasForeignKey(u => u.Application_Id);
            modelBuilder.Entity<User_Application_Privileges>().HasOne(u => u.Company).WithMany().HasForeignKey(u => u.Emp_Id);
            //User_Application_Rol
            modelBuilder.Entity<User_Application_Rol>().HasKey(u => u.Id);
            modelBuilder.Entity<User_Application_Rol>().HasOne(u => u.User).WithMany().HasForeignKey(u => u.User_Id);
            modelBuilder.Entity<User_Application_Rol>().HasOne(u => u.Roles).WithMany().HasForeignKey(u => u.Rol_Id);
            modelBuilder.Entity<User_Application_Rol>().HasOne(u => u.Application).WithMany().HasForeignKey(u => u.Application_Id);
            modelBuilder.Entity<User_Application_Rol>().HasOne(u => u.Company).WithMany().HasForeignKey(u => u.Emp_Id);

        }
    }


}
