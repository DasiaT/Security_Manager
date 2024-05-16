using Manager_Security_BackEnd.Models.Element_Pages;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IElement_Page
    {
        Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> GetElementPageAsync(Comun_Filters value);
        Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> PostElementPageAsync(Element_Page_Request_Post value);
        Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> PatchElementPageAsync(Element_Page_Request_Patch value);
        Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> DeleteElementPageAsync(Element_Page_Request_Delete value);
    }
}
