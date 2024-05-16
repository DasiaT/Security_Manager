using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Pags;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IPages
    {
        Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> GetPagesAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> PostPagesAsync(Pages_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> PatchPagesAsync(Pages_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> DeletePagesAsync(Pages_Request_Delete value);
    }
}
