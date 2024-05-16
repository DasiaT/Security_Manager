using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Access_Page_Rols;
using Manager_Security_BackEnd.Models.Applications_Rol_Privileges;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Access_Page_Rols
{
    public class Access_Page_Rol_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Access_Page_Rol_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }

        public async Task<List<ErrorServices>> Access_Page_Rol_Valid_Post(Access_Page_Rol_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (value.Application_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id field cannot be empty.", 400));
            }

            if (value.Rol_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Rol id field cannot be empty.", 400));
            }

            if (value.Page_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Pages Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoApplication = await _context.Application.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id);

                if (validoApplication == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Id not exists, insert a valid.", 400));
                }

                var validoPages = await _context.Pages.FirstOrDefaultAsync(x => x.Page_Id == value.Page_Id);

                if (validoPages == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Pages Id not exists, insert a valid.", 400));
                }

                var validoRol = await _context.Roles.FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id);

                if (validoRol == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Rol Id not exists, insert a valid.", 400));
                }

                var validoExist = await _context.Access_Page_Rol
                    .FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id && x.Application_Id == value.Application_Id && x.Page_Id == value.Page_Id);

                if (validoExist != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Access Rol Pages exists, can't have duplicated.", 400));
                }
            }

            return errores;
        }

        public async Task<List<ErrorServices>> Access_Page_Rol_Valid_Patch(Access_Page_Rol_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Application_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id field cannot be empty.", 400));
            }

            if (value.Rol_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Rol id field cannot be empty.", 400));
            }

            if (value.Page_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Pages Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoApplication = await _context.Application.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id);

                if (validoApplication == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Id not exists, insert a valid.", 400));
                }

                var validoPrivileges = await _context.Pages.FirstOrDefaultAsync(x => x.Page_Id == value.Page_Id);

                if (validoPrivileges == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Pages Id not exists, insert a valid.", 400));
                }

                var validoRol = await _context.Roles.FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id);

                if (validoRol == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Rol Id not exists, insert a valid.", 400));
                }

                var validoExist = await _context.Application_Rol_Privileges
                    .FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id && x.Application_Id == value.Application_Id && x.Privilege_Id == value.Page_Id);

                if (validoExist != null && validoExist.Id != value.Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Access Rol Pages exists, can't have duplicated.", 400));
                }
            }

            return errores;
        }
    }
}
