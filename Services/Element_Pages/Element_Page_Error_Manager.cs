using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Element_Pages;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Element_Pages
{
    public class Element_Page_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Element_Page_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }
        public async Task<List<ErrorServices>> Element_Page_Valid_Post(Element_Page_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.Name_Element) || string.IsNullOrWhiteSpace(value.Name_Element))
            {
                errores.Add(_errorService.GetBadRequestException("The Element Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoRol = await _context.Element_Page.FirstOrDefaultAsync(x => x.Name_Element == value.Name_Element);

                if (validoRol != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Element Name already exists in another.", 400));
                }

            }

            return errores;
        }

        public async Task<List<ErrorServices>> Element_Page_Valid_Patch(Element_Page_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Element_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Name_Element) || string.IsNullOrWhiteSpace(value.Name_Element))
            {
                errores.Add(_errorService.GetBadRequestException("The Element Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var valido_Privileges = await _context.Element_Page.FirstOrDefaultAsync(x => x.Element_Id == value.Element_Id);

                if (valido_Privileges == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Element Id not exists, insert a valid.", 400));
                }

                var validoName = await _context.Privileges.FirstOrDefaultAsync(x => x.Name == value.Name_Element);

                if (validoName != null && validoName.Id != value.Element_Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Element Name already exists in another privileges.", 400));
                }
            }


            return errores;
        }
    }
}
