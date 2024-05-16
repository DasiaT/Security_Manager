using Manager_Security_BackEnd.Models.Applications_Rol_Privileges;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IApplication_Rol_Privileges
    {
        Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> GetApplicationRolPrivilegesAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> PostApplicationRolPrivilegesAsync(Application_Rol_Privileges_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> PatchApplicationRolPrivilegesAsync(Application_Rol_Privileges_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> DeleteApplicationRolPrivilegesAsync(Application_Rol_Privileges_Request_Delete value);
    }
}
