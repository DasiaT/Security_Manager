using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Information_Users;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Manager_Security_BackEnd.Services.Information_User_Services
{
    public class Information_User_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Information_User_Error_Manager(conectionDBcontext context,IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }
        public async Task<List<ErrorServices>> Information_User_Valid(Information_User_Request value)
        {
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Name field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Surnames) || string.IsNullOrWhiteSpace(value.Surnames))
            {
                errores.Add(_errorService.GetBadRequestException("The Surnames field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.DNI) || string.IsNullOrWhiteSpace(value.DNI))
            {
                errores.Add(_errorService.GetBadRequestException("The DNI field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Email) || string.IsNullOrWhiteSpace(value.Email))
            {
                errores.Add(_errorService.GetBadRequestException("The Email field cannot be empty.", 400));
            }

            if (value.Id_workstation == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Id_Workstation field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoDNI = await _context.Information_User.FirstOrDefaultAsync(x => x.DNI == value.DNI);

                if (validoDNI != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The DNI already exists in another user.", 400));
                }

                string patternEmail = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

                Boolean ValidoFormatoEmail =  Regex.IsMatch(value.Email, patternEmail);

                if (ValidoFormatoEmail == false)
                {
                    errores.Add(_errorService.GetBadRequestException("The Email format is not valid.", 400));
                }

                var validoEmail = await _context.Information_User.FirstOrDefaultAsync(x => x.Email == value.Email);

                if (validoEmail != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Email already exists in another user.", 400));
                }

                var validoWorkstation = await _context.Information_Workstation.FirstOrDefaultAsync(x => x.Id == value.Id_workstation);

                if (validoWorkstation == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Id_Workstation not exists, insert a valid.", 400));
                }
            }
            

            return errores;
        }

        public async Task<List<ErrorServices>> Information_User_ValidPatch(Information_User_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Name field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Surnames) || string.IsNullOrWhiteSpace(value.Surnames))
            {
                errores.Add(_errorService.GetBadRequestException("The Surnames field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.DNI) || string.IsNullOrWhiteSpace(value.DNI))
            {
                errores.Add(_errorService.GetBadRequestException("The DNI field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Email) || string.IsNullOrWhiteSpace(value.Email))
            {
                errores.Add(_errorService.GetBadRequestException("The Email field cannot be empty.", 400));
            }

            if (value.Id_workstation == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Id_Workstation field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoID = await _context.Information_User.FirstOrDefaultAsync(x => x.Id == value.Id);

                if (validoID == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Id not exists, insert a valid.", 400));
                }

                var validoDNI = await _context.Information_User.FirstOrDefaultAsync(x => x.DNI == value.DNI);

                if (validoDNI != null && validoDNI.Id != value.Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The DNI already exists in another user.", 400));
                }

                string patternEmail = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

                Boolean ValidoFormatoEmail = Regex.IsMatch(value.Email, patternEmail);

                if (ValidoFormatoEmail == false)
                {
                    errores.Add(_errorService.GetBadRequestException("The Email format is not valid.", 400));
                }

                var validoEmail = await _context.Information_User.FirstOrDefaultAsync(x => x.Email == value.Email);

                if (validoEmail != null && validoEmail.Id != value.Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Email already exists in another user.", 400));
                }

                var validoWorkstation = await _context.Information_Workstation.FirstOrDefaultAsync(x => x.Id == value.Id_workstation);

                if (validoWorkstation == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Id_Workstation not exists, insert a valid.", 400));
                }
            }


            return errores;
        }
    }
}
