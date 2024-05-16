using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Rols;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Rol_Services
{
    public class Roles_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Roles_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }
        public async Task<List<ErrorServices>> Roles_Valid_Post(Roles_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Rol Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoRol = await _context.Roles.FirstOrDefaultAsync(x => x.Name == value.Name);

                if (validoRol != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Rol Name already exists in another user.", 400));
                }

            }

            return errores;
        }

        public async Task<List<ErrorServices>> Roles_Valid_Patch(Roles_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Rol_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Rol Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Application Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoRoles = await _context.Roles.FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id);

                if (validoRoles == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Rol Id not exists, insert a valid.", 400));
                }

                var validoName = await _context.Roles.FirstOrDefaultAsync(x => x.Name == value.Name);

                if (validoName != null && validoName.Rol_Id != value.Rol_Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Rol Name already exists in another user.", 400));
                }
            }


            return errores;
        }
    }
}
