using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IApplication
    {
        Task<(bool isError, List<ErrorServices> error, Application_Response? result)> GetApplicationAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Application_Response? result)> PostApplicationAsync(Application_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Application_Response? result)> PatchApplicationAsync(Application_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Application_Response? result)> DeleteApplicationAsync(Application_Request_Delete value);
    }
}
