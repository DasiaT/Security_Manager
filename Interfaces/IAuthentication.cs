using Manager_Security_BackEnd.Models.Authentications;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IAuthentication
    {
        Task<(bool isError, List<ErrorServices> error, Authentication_Response? result)> GetAuthenticationAsync(Comun_Filters value);
    }
}
