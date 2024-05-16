using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Users_Applications_Rols;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IUser_Application_Rol
    {
        Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> GetUserApplicationRolAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> PostUserApplicationRolAsync(User_Application_Rol_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> PatchUserApplicationRolAsync(User_Application_Rol_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> DeleteUserApplicationRolAsync(User_Application_Rol_Request_Delete value);
    }
}
