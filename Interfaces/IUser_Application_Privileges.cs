using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Users_Applications_Privileges;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IUser_Application_Privileges
    {
        Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> GetUserApplicationPrivilegesAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> PostUserApplicationPrivilegesAsync(User_Application_Privileges_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> PatchUserApplicationPrivilegesAsync(User_Application_Privileges_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> DeleteUserApplicationPrivilegesAsync(User_Application_Privileges_Request_Delete value);
    }
}
