using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Users;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.User_Services
{
    public class User_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public User_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }
        public async Task<List<ErrorServices>> User_Valid_Post(User_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.User_Name) || string.IsNullOrWhiteSpace(value.User_Name))
            {
                errores.Add(_errorService.GetBadRequestException("The User Name field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Password) || string.IsNullOrWhiteSpace(value.Password))
            {
                errores.Add(_errorService.GetBadRequestException("The Password field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var valido_User_Exist = await _context.User.FirstOrDefaultAsync(x => x.User_Name == value.User_Name);

                if (valido_User_Exist != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The User Name already exists in another user.", 400));
                }
            }

            return errores;
        }

        public async Task<List<ErrorServices>> User_Valid_Patch(User_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (errores.Count == 0)
            {
                var valido_User_Exist = await _context.User.FirstOrDefaultAsync(x => x.User_Name == value.User_Name);

                if (valido_User_Exist != null && valido_User_Exist.Id != value.Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The User Name already exists in another user.", 400));
                }
            }

            return errores;
        }
    }
}
