using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Applications_Rol_Privileges;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Application_Rol_Privileges_Services
{
    public class Application_Rol_Privileges_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Application_Rol_Privileges_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }

        public async Task<List<ErrorServices>> Application_Rol_Privileges_Valid_Post(Application_Rol_Privileges_Request_Post value)
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

            if (value.Privilege_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Privilege Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoApplication = await _context.Application.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id);

                if (validoApplication == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Id not exists, insert a valid.", 400));
                }
               
                var validoPrivileges = await _context.Privileges.FirstOrDefaultAsync(x => x.Id == value.Privilege_Id);

                if (validoPrivileges == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Privileges Id not exists, insert a valid.", 400));
                }

                var validoRol = await _context.Roles.FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id);

                if (validoRol == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Rol Id not exists, insert a valid.", 400));
                }

                var validoExist = await _context.Application_Rol_Privileges
                    .FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id && x.Application_Id == value.Application_Id && x.Privilege_Id == value.Privilege_Id);

                if (validoExist != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Rol Privileges exists, can't have duplicated.", 400));
                }
            }

            return errores;
        }

        public async Task<List<ErrorServices>> Application_Rol_Privileges_Valid_Patch(Application_Rol_Privileges_Request_Patch value)
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

            if (value.Privilege_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Privilege Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoApplication = await _context.Application.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id);

                if (validoApplication == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Id not exists, insert a valid.", 400));
                }

                var validoPrivileges = await _context.Privileges.FirstOrDefaultAsync(x => x.Id == value.Privilege_Id);

                if (validoPrivileges == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Privileges Id not exists, insert a valid.", 400));
                }

                var validoRol = await _context.Roles.FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id);

                if (validoRol == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Rol Id not exists, insert a valid.", 400));
                }

                var validoExist = await _context.Application_Rol_Privileges
                    .FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id && x.Application_Id == value.Application_Id && x.Privilege_Id == value.Privilege_Id);

                if (validoExist != null && validoExist.Id != value.Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Rol Privileges exists, can't have duplicated.", 400));
                }
            }

            return errores;
        }
    }
}
