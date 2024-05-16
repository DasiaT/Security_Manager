using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Privilegs;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IPrivileges
    {
        Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> GetPrivilegesAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> PostPrivilegesAsync(Privileges_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> PatchPrivilegesAsync(Privileges_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> DeletePrivilegesAsync(Privileges_Request_Delete value);
    }
}
