using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Users_Applications_Privileges;
using Manager_Security_BackEnd.Models.Users_Applications_Rols;
using Manager_Security_BackEnd.Services.Error_Services;
using Manager_Security_BackEnd.Services.Users_Applications_Rols;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Users_Applications_Privileges
{
    public class UserApplicationPrivilegesSevices: IUser_Application_Privileges
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly User_Application_Privileges_Error_Manager _user_application_privileges_error_manager;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public UserApplicationPrivilegesSevices(conectionDBcontext context, IError errorService, User_Application_Privileges_Error_Manager user_application_privileges_error_manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _user_application_privileges_error_manager = user_application_privileges_error_manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> GetUserApplicationPrivilegesAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            User_Application_Privileges_Response? results = new();
            List<User_Application_Privileges>? user_Application_Privileges = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListUserApplicationPrivileges_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            user_Application_Privileges = await _generate_Cache_Key.Buscar_En_CacheAsync<User_Application_Privileges>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (user_Application_Privileges?.Count > 0)
            {
                results.Result = user_Application_Privileges?.OrderByDescending(x => x.Id).ToList();
                results.Count = user_Application_Privileges?.Count;

                return (false, errores, results);
            }
            else
            {
                if (value.Take > 0)//PARA USAR LIMIT DE SQL
                {
                    take = value.Take;
                }

                if (value.Skip > 0)//PARA SALTAR LAS FILAS ES EL OFFSET DE SQL
                {
                    skip = value.Skip;
                }

                if (value.Id != null && value.Search != null)
                {
                    user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => (x.User.User_Name.ToLower().Contains(value.Search.ToLower()) || x.User.Information_User.Name.ToLower().Contains(value.Search.ToLower())) && x.Id == value.Id)
                        .Skip(skip).Take(take).OrderByDescending(x => x.Emp_Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => (x.User.User_Name.ToLower().Contains(value.Search.ToLower()) || x.User.Information_User.Name.ToLower().Contains(value.Search.ToLower())))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }

                if (user_Application_Privileges != null)
                {
                    results.Result = user_Application_Privileges;
                    results.Count = user_Application_Privileges.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, user_Application_Privileges);
                }

                return (false, errores, results);
            }
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> PostUserApplicationPrivilegesAsync(User_Application_Privileges_Request_Post value)
        {
            User_Application_Privileges_Response? results = new();
            List<User_Application_Privileges>? user_Application_Privileges = new();
            List<ErrorServices> errores = new();

            errores = await _user_application_privileges_error_manager.User_Application_Privileges_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_user_application_privileges = new User_Application_Privileges
            {
                Application_Id = value.Application_Id,
                Emp_Id = value.Emp_Id,
                Privileges_Id = value.Privileges_Id,
                User_Id = value.User_Id,
                Date_Insert = currentDateUtc,
            };

            _context.User_Application_Privileges.Add(new_user_application_privileges);

            await _context.SaveChangesAsync();

            user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => x.Application_Id == value.Application_Id && x.Emp_Id == value.Emp_Id && x.User_Id == value.User_Id && x.Privileges_Id == value.Privileges_Id).ToListAsync();

            if (user_Application_Privileges != null)
            {
                results.Result = user_Application_Privileges;
                results.Count = user_Application_Privileges.Count;

                user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .OrderByDescending(x => x.Id).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUserApplicationPrivileges_0_0", user_Application_Privileges);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> PatchUserApplicationPrivilegesAsync(User_Application_Privileges_Request_Patch value)
        {
            User_Application_Privileges_Response? results = new();
            List<User_Application_Privileges> user_Application_Privileges = [];
            List<ErrorServices> errores = [];

            errores = await _user_application_privileges_error_manager.User_Application_Privileges_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var user_Application_Privilegs = await _context.User_Application_Privileges.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (user_Application_Privilegs == null)
            {
                errores.Add(_errorService.GetBadRequestException("The User Application Privileges Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            user_Application_Privilegs.Application_Id = value.Application_Id ?? user_Application_Privilegs.Application_Id;
            user_Application_Privilegs.Emp_Id = value.Emp_Id ?? user_Application_Privilegs.Emp_Id;
            user_Application_Privilegs.Privileges_Id = value.Privileges_Id ?? user_Application_Privilegs.Privileges_Id;
            user_Application_Privilegs.User_Id = value.User_Id ?? user_Application_Privilegs.User_Id;
            user_Application_Privilegs.Date_Update = currentDateUtc;

            await _context.SaveChangesAsync();

            user_Application_Privileges = await _context.User_Application_Privileges.Where(x => x.Id == value.Id).ToListAsync();

            if (user_Application_Privileges != null)
            {
                results.Result = user_Application_Privileges;
                results.Count = user_Application_Privileges.Count;

                user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .OrderByDescending(x => x.Id).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUserApplicationPrivileges_0_0", user_Application_Privileges);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Application_Privileges_Response? result)> DeleteUserApplicationPrivilegesAsync(User_Application_Privileges_Request_Delete value)
        {
            User_Application_Privileges_Response? results = new();
            List<User_Application_Privileges> user_Application_Privileges = [];
            List<ErrorServices> errores = [];

            var user_Application_Privelegs = await _context.User_Application_Privileges.FirstOrDefaultAsync(e => e.Id == value.Id);

            if (user_Application_Privelegs == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.User_Application_Privileges.Remove(user_Application_Privelegs);

            await _context.SaveChangesAsync();

            if (user_Application_Privileges != null)
            {
                results.Result = user_Application_Privileges;
                results.Count = user_Application_Privileges.Count;

                user_Application_Privileges = await _context.User_Application_Privileges
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Privileges)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .OrderByDescending(x => x.Id).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUserApplicationPrivileges_0_0", user_Application_Privileges);
            }

            return (false, errores, results);
        }
    }
}
