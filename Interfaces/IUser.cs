using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Users;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IUser
    {
        Task<(bool isError, List<ErrorServices> error, User_Response? result)> GetUserAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, User_Response_Login? result)> PostUserLoginAsync(User_Request_Post_Login value);
        Task<(bool isError, List<ErrorServices> error, User_Response? result)> PostUserAsync(User_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, User_Response? result)> PatchUserAsync(User_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, User_Response? result)> DeleteUserAsync(User_Request_Delete value);
    }
}
