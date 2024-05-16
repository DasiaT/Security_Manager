

using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Users_Applications_Rols;
using Manager_Security_BackEnd.Services.Application_Services;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Users_Applications_Rols
{
    public class UserApplicationRolSevices : IUser_Application_Rol
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly User_Application_Rol_Error_Manager _user_application_rol_error_manager;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public UserApplicationRolSevices(conectionDBcontext context, IError errorService, User_Application_Rol_Error_Manager user_Application_Rol_Error, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _user_application_rol_error_manager = user_Application_Rol_Error;
            _generate_Cache_Key = generate_Cache_Key;
        }

        public async Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> GetUserApplicationRolAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            User_Application_Rol_Response? results = new();
            List<User_Application_Rol>? user_Application_Rols = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListUserApplicationRol_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            user_Application_Rols = await _generate_Cache_Key.Buscar_En_CacheAsync<User_Application_Rol>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (user_Application_Rols?.Count > 0)
            {
                results.Result = user_Application_Rols?.OrderByDescending(x => x.Id).ToList();
                results.Count = user_Application_Rols?.Count;

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
                    user_Application_Rols = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => (x.User.User_Name.ToLower().Contains(value.Search.ToLower()) || x.User.Information_User.Name.ToLower().Contains(value.Search.ToLower())) && x.Id == value.Id)
                        .Skip(skip).Take(take).OrderByDescending(x => x.Emp_Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    user_Application_Rols = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    user_Application_Rols = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => (x.User.User_Name.ToLower().Contains(value.Search.ToLower()) || x.User.Information_User.Name.ToLower().Contains(value.Search.ToLower())))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    user_Application_Rols = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }

                if (user_Application_Rols != null)
                {
                    results.Result = user_Application_Rols;
                    results.Count = user_Application_Rols.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, user_Application_Rols);
                }

                return (false, errores, results);
            }
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> PostUserApplicationRolAsync(User_Application_Rol_Request_Post value)
        {
            User_Application_Rol_Response? results = new();
            List<User_Application_Rol>? user_Application_Rol = new();
            List<ErrorServices> errores = new();

            errores = await _user_application_rol_error_manager.User_Application_Rol_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_user_application_rol = new User_Application_Rol
            {
                Application_Id = value.Application_Id,
                Emp_Id = value.Emp_Id,
                Rol_Id = value.Rol_Id,
                User_Id = value.User_Id,
                Date_Insert = currentDateUtc,
                Date_Update = currentDateUtc,
               
            };

            _context.User_Application_Rol.Add(new_user_application_rol);

            await _context.SaveChangesAsync();

            user_Application_Rol = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .Where(x => x.Application_Id == value.Application_Id && x.Emp_Id == value.Emp_Id && x.User_Id == value.User_Id && x.Rol_Id == value.Rol_Id).ToListAsync();

            if (user_Application_Rol != null)
            {
                results.Result = user_Application_Rol;
                results.Count = user_Application_Rol.Count;

                user_Application_Rol = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .OrderByDescending(x => x.Id).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUserApplicationRol_0_0", user_Application_Rol);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> PatchUserApplicationRolAsync(User_Application_Rol_Request_Patch value)
        {
            User_Application_Rol_Response? results = new();
            List<User_Application_Rol> user_Application_Rol = new();
            List<ErrorServices> errores = new();

            //errores = await _application_Error_Manager.Application_Valid_Patch(value);

            //if (errores.Count > 0)
            //{
            //    return (true, errores, null);
            //}

            var user_Application_Rols = await _context.User_Application_Rol.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (user_Application_Rols == null)
            {
                errores.Add(_errorService.GetBadRequestException("The User Application Rol Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            user_Application_Rols.Application_Id = value.Application_Id ?? user_Application_Rols.Application_Id;
            user_Application_Rols.Emp_Id = value.Emp_Id ?? user_Application_Rols.Emp_Id;
            user_Application_Rols.Rol_Id = value.Rol_Id ?? user_Application_Rols.Rol_Id;
            user_Application_Rols.User_Id = value.User_Id ?? user_Application_Rols.User_Id;
            user_Application_Rols.Date_Update = currentDateUtc;

            await _context.SaveChangesAsync();

            user_Application_Rol = await _context.User_Application_Rol.Where(x => x.Application_Id == value.Application_Id).ToListAsync();

            if (user_Application_Rol != null)
            {
                results.Result = user_Application_Rol;
                results.Count = user_Application_Rol.Count;

                user_Application_Rol = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .OrderByDescending(x => x.Id).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUserApplicationRol_0_0", user_Application_Rol);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Application_Rol_Response? result)> DeleteUserApplicationRolAsync(User_Application_Rol_Request_Delete value)
        {
            User_Application_Rol_Response? results = new();
            List<User_Application_Rol> user_Application_Rol = new();
            List<ErrorServices> errores = new();

            var user_Application_Rols = await _context.User_Application_Rol.FirstOrDefaultAsync(e => e.Id == value.Id);

            if (user_Application_Rols == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.User_Application_Rol.Remove(user_Application_Rols);

            await _context.SaveChangesAsync();

            if (user_Application_Rol != null)
            {
                results.Result = user_Application_Rol;
                results.Count = user_Application_Rol.Count;

                user_Application_Rol = await _context.User_Application_Rol
                        .Include(x => x.Application)
                        .Include(x => x.Company)
                        .Include(x => x.Roles)
                        .Include(x => x.User)
                        .Include(x => x.User.Information_User)
                        .OrderByDescending(x => x.Id).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUserApplicationRol_0_0", user_Application_Rol);
            }

            return (false, errores, results);
        }
    }
}
