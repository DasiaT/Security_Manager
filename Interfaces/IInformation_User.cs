using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Information_Users;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IInformation_User
    {
        Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> GetInformationUserAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> PostInformationUserAsync(Information_User_Request value);
        Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> PatchInformationUserAsync(Information_User_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> DeleteInformationUserAsync(Information_User_Request_Delete value);
    }
}
