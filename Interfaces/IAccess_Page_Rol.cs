using Manager_Security_BackEnd.Models.Access_Page_Rols;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IAccess_Page_Rol
    {
        Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> GetAccessPageRolAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> PostAccessPageRolAsync(Access_Page_Rol_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> PatchAccessPageRolAsync(Access_Page_Rol_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> DeleteAccessPageRolAsync(Access_Page_Rol_Request_Delete value);
    }
}
