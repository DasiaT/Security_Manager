using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals.General_Valid_Exist;
using Manager_Security_BackEnd.Models.Users_Applications_Rols;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Users_Applications_Rols
{
    public class User_Application_Rol_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Valid_Application_Exist _general_Valid_Application_Exist;
        private readonly General_Valid_User_Exist _user_Valid_User_Exist;
        private readonly General_Valid_Company_Exist _general_Valid_Company_Exist;
        private readonly General_Valid_Rol_Exist _general_Valid_Rol_Exist;
        public User_Application_Rol_Error_Manager(conectionDBcontext context, IError errorService, General_Valid_Application_Exist general_Valid_Application_Exist, General_Valid_User_Exist user_Valid_User_Exist, General_Valid_Company_Exist general_Valid_Company_Exist, General_Valid_Rol_Exist general_Valid_Rol_Exist)
        {
            _context = context;
            _errorService = errorService;
            _general_Valid_Application_Exist = general_Valid_Application_Exist;
            _user_Valid_User_Exist = user_Valid_User_Exist;
            _general_Valid_Company_Exist = general_Valid_Company_Exist;
            _general_Valid_Rol_Exist = general_Valid_Rol_Exist;
        }
        public async Task<List<ErrorServices>> User_Application_Rol_Valid_Post(User_Application_Rol_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (value.User_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The User Id field cannot be empty.", 400));
            }

            if (value.Emp_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Emp Id field cannot be empty.", 400));
            }

            if (value.Application_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id field cannot be empty.", 400));
            }

            if (value.Rol_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Rol Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoExistEquals = await _context.User_Application_Rol
                    .FirstOrDefaultAsync(x => x.User_Id == value.User_Id && x.Application_Id == value.Application_Id && x.Emp_Id == value.Emp_Id && x.Rol_Id == value.Rol_Id);

                if (validoExistEquals != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The User Application Rol exists.", 400));
                }
                else
                {
                    Boolean valido_User_Exist = await _user_Valid_User_Exist.IsValidExistUserAsync(value.User_Id);

                    if (valido_User_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The User Id not exists.", 400));
                    }

                    Boolean valido_Company_Exist = await _general_Valid_Company_Exist.IsValidExistCompanyAsync(value.Emp_Id);

                    if (valido_Company_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The Company Id not exists.", 400));
                    }

                    Boolean valido_Application_Exist = await _general_Valid_Application_Exist.IsValidExistApplicationAsync(value.Application_Id);

                    if (valido_Application_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The Application Id not exists.", 400));
                    }

                    Boolean valido_Rol_Exist = await _general_Valid_Rol_Exist.IsValidExistRolesAsync(value.Rol_Id);

                    if (valido_Rol_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The Rol Id not exists.", 400));
                    }
                }
            }

            return errores;
        }
        public async Task<List<ErrorServices>> User_Application_Rol_Valid_Patch(User_Application_Rol_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.User_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The User Id field cannot be empty.", 400));
            }

            if (value.Emp_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Emp Id field cannot be empty.", 400));
            }

            if (value.Application_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id field cannot be empty.", 400));
            }

            if (value.Rol_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Rol Id field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoExistEquals = await _context.User_Application_Rol
                   .FirstOrDefaultAsync(x => x.User_Id == value.User_Id && x.Application_Id == value.Application_Id && x.Emp_Id == value.Emp_Id && x.Rol_Id == value.Rol_Id);

                if (validoExistEquals != null && validoExistEquals.Id != value.Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The User Application Rol exists.", 400));
                }
                else
                {
                    Boolean valido_User_Exist = await _user_Valid_User_Exist.IsValidExistUserAsync(value.User_Id ?? 0);

                    if (valido_User_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The User Id not exists.", 400));
                    }

                    Boolean valido_Company_Exist = await _general_Valid_Company_Exist.IsValidExistCompanyAsync(value.Emp_Id ?? 0);

                    if (valido_Company_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The Company Id not exists.", 400));
                    }

                    Boolean valido_Application_Exist = await _general_Valid_Application_Exist.IsValidExistApplicationAsync(value.Application_Id ?? 0);

                    if (valido_Application_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The Application Id not exists.", 400));
                    }

                    Boolean valido_Rol_Exist = await _general_Valid_Rol_Exist.IsValidExistRolesAsync(value.Rol_Id ?? 0);

                    if (valido_Rol_Exist)
                    {
                        errores.Add(_errorService.GetBadRequestException("The Rol Id not exists.", 400));
                    }
                }
            }

            return errores;
        }
    }
}
