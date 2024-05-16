using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Applications_Rol_Privileges;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Application_Rol_Privileges_Services
{
    public class ApplicationRolPrivilegesServices : IApplication_Rol_Privileges
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly Application_Rol_Privileges_Error_Manager _application_Rol_Privileges_Error_Manager;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public ApplicationRolPrivilegesServices(conectionDBcontext context, IError errorService, Application_Rol_Privileges_Error_Manager application_Rol_Privileges_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _application_Rol_Privileges_Error_Manager = application_Rol_Privileges_Error_Manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> GetApplicationRolPrivilegesAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Application_Rol_Privileges_Response? results = new();
            List<Application_Rol_Privileges>? application_Rol_Privileges = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListApplication_Rol_Privileges_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            application_Rol_Privileges = await _generate_Cache_Key.Buscar_En_CacheAsync<Application_Rol_Privileges>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (application_Rol_Privileges?.Count > 0)
            {
                results.Result = application_Rol_Privileges?.OrderByDescending(x => x.Application_Id).ToList();
                results.Count = application_Rol_Privileges?.Count;

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
                    application_Rol_Privileges = await _context.Application_Rol_Privileges.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges).Where(x => x.Id == value.Id && x.Application.Application_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    application_Rol_Privileges = await _context.Application_Rol_Privileges.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges).Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    application_Rol_Privileges = await _context.Application_Rol_Privileges.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges).Where(x => x.Application.Application_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    application_Rol_Privileges = await _context.Application_Rol_Privileges.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges).Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }

                if (application_Rol_Privileges != null)
                {
                    results.Result = application_Rol_Privileges;
                    results.Count = application_Rol_Privileges.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, application_Rol_Privileges);
                }

                return (false, errores, results);
            }
        }
        public async Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> PostApplicationRolPrivilegesAsync(Application_Rol_Privileges_Request_Post value)
        {
            Application_Rol_Privileges_Response? results = new();
            List<Application_Rol_Privileges> application_Rol_Privileges = [];
            List<ErrorServices> errores = [];

            errores = await _application_Rol_Privileges_Error_Manager.Application_Rol_Privileges_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_application = new Application_Rol_Privileges
            {
                Rol_Id = value.Rol_Id,
                Privilege_Id = value.Privilege_Id,
                Application_Id = value.Application_Id,
                Date_Insert = currentDateUtc,
            };

            _context.Application_Rol_Privileges.Add(new_application);

            await _context.SaveChangesAsync();

            application_Rol_Privileges = await _context.Application_Rol_Privileges.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges)
                .Where(x => x.Application_Id == value.Application_Id && x.Privilege_Id == value.Privilege_Id && x.Rol_Id == value.Rol_Id).ToListAsync();

            if (application_Rol_Privileges != null)
            {
                results.Result = application_Rol_Privileges;
                results.Count = application_Rol_Privileges.Count;

                application_Rol_Privileges = await _context.Application_Rol_Privileges
                    .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListApplication_Rol_Privileges_0_0", application_Rol_Privileges);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> PatchApplicationRolPrivilegesAsync(Application_Rol_Privileges_Request_Patch value)
        {
            Application_Rol_Privileges_Response? results = new();
            List<Application_Rol_Privileges> application_Rol_Privileges = new();
            List<ErrorServices> errores = new();

            errores = await _application_Rol_Privileges_Error_Manager.Application_Rol_Privileges_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var applications = await _context.Application_Rol_Privileges.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (applications == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Rol Privileges Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            applications.Application_Id = value.Application_Id ?? applications.Application_Id;
            applications.Rol_Id = value.Rol_Id ?? applications.Rol_Id;
            applications.Privilege_Id = value.Privilege_Id ?? applications.Privilege_Id;
            applications.Date_Update = currentDateUtc;

            await _context.SaveChangesAsync();

            application_Rol_Privileges = await _context.Application_Rol_Privileges
                .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges)
                .Where(x => x.Application_Id == value.Application_Id).ToListAsync();

            if (application_Rol_Privileges != null)
            {
                results.Result = application_Rol_Privileges;
                results.Count = application_Rol_Privileges.Count;

                application_Rol_Privileges = await _context.Application_Rol_Privileges
                    .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListApplication_Rol_Privileges_0_0", application_Rol_Privileges);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Application_Rol_Privileges_Response? result)> DeleteApplicationRolPrivilegesAsync(Application_Rol_Privileges_Request_Delete value)
        {
            Application_Rol_Privileges_Response? results = new();
            List<Application_Rol_Privileges> application_Rol_Privileges = [];
            List<ErrorServices> errores = [];

            var applications = await _context.Application_Rol_Privileges.FirstOrDefaultAsync(e => e.Id == value.Id);

            if (applications == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Rol Privileges Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Application_Rol_Privileges.Remove(applications);

            await _context.SaveChangesAsync();

            if (application_Rol_Privileges != null)
            {
                results.Result = application_Rol_Privileges;
                results.Count = application_Rol_Privileges.Count;

                application_Rol_Privileges = await _context.Application_Rol_Privileges
                    .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Privileges).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListApplication_Rol_Privileges_0_0", application_Rol_Privileges);
            }

            return (false, errores, results);
        }

    }
}
