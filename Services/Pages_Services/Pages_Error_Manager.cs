using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Information_Users;
using Manager_Security_BackEnd.Models.Pags;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Manager_Security_BackEnd.Services.Pages_Services
{
    public class Pages_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Pages_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }
        public async Task<List<ErrorServices>> Pages_User_Valid_Post(Pages_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Name field cannot be empty.", 400));
            }

            if (value.Application_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var ValidoApplication = await _context.Application.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id);

                if (ValidoApplication == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application not exists.", 400));
                }

                var validoNombre = await _context.Pages.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id && x.Name == value.Name);

                if (validoNombre != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Name already exists on another page of the same application.", 400));
                }
            }

            return errores;
        }

        public async Task<List<ErrorServices>> Pages_User_Valid_Patch(Pages_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Page_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Page Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Name field cannot be empty.", 400));
            }

            if (value.Application_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var ValidoPages = await _context.Pages.FirstOrDefaultAsync(x => x.Page_Id == value.Page_Id);

                if (ValidoPages == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Page Id not exists.", 400));
                }

                var ValidoApplication = await _context.Application.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id);

                if (ValidoApplication == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application not exists.", 400));
                }

                var validoNombre = await _context.Pages.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id && x.Name == value.Name);

                if (validoNombre != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Name already exists on another page of the same application.", 400));
                }
            }

            return errores;
        }
    }
}
