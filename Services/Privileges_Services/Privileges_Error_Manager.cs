using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Privilegs;
using Manager_Security_BackEnd.Models.Rols;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Privileges_Services
{
    public class Privileges_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Privileges_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }
        public async Task<List<ErrorServices>> Privileges_Valid_Post(Privileges_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Privileges Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoRol = await _context.Privileges.FirstOrDefaultAsync(x => x.Name == value.Name);

                if (validoRol != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Privileges Name already exists in another.", 400));
                }

            }

            return errores;
        }

        public async Task<List<ErrorServices>> Privileges_Valid_Patch(Privileges_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Privileges Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var valido_Privileges = await _context.Privileges.FirstOrDefaultAsync(x => x.Id == value.Id);

                if (valido_Privileges == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Privileges Id not exists, insert a valid.", 400));
                }

                var validoName = await _context.Privileges.FirstOrDefaultAsync(x => x.Name == value.Name);

                if (validoName != null && validoName.Id != value.Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Privileges Name already exists in another privileges.", 400));
                }
            }


            return errores;
        }
    }
}
