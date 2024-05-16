using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Workstation;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IWorkstation
    {
        Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> GetWorkstationsAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> PostWorkstationsAsync(Information_Workstation_Request value);
        Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> PatchWorkstationsAsync(Information_Workstation_Model value);
        Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> DeleteWorkstationsAsync(Information_Workstation_Request_Delete value);
    }
}
