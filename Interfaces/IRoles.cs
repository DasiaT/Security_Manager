using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Rols;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IRoles
    {
        Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> GetRolesAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> PostRolesAsync(Roles_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> PatchRolesAsync(Roles_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> DeleteRolesAsync(Roles_Request_Delete value);
    }
}
