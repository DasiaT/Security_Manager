using Manager_Security_BackEnd.Models.Companys;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface ICompany
    {
        Task<(bool isError, List<ErrorServices> error, Company_Response? result)> GetCompanyAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Company_Response? result)> PostCompanyAsync(Company_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Company_Response? result)> PatchCompanyAsync(Company_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Company_Response? result)> DeleteCompanyAsync(Company_Request_Delete value);
    }
}
