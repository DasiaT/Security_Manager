using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Rols;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Rol_Services
{
    public class RolesServices : IRoles
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        private readonly Roles_Error_Manager _roles_Error_Manager;
        public RolesServices(conectionDBcontext context, IError errorService, Roles_Error_Manager roles_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _roles_Error_Manager = roles_Error_Manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> GetRolesAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Roles_Response? results = new();
            List<Roles>? roles = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListRoles_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            roles = await _generate_Cache_Key.Buscar_En_CacheAsync<Roles>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (roles?.Count > 0)
            {
                results.Result = roles?.OrderByDescending(x => x.Rol_Id).ToList();
                results.Count = roles?.Count;

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
                    roles = await _context.Roles.Where(x => x.Rol_Id == value.Id && x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Rol_Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    roles = await _context.Roles.Where(x => x.Rol_Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    roles = await _context.Roles.Where(x => x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Rol_Id).ToListAsync();
                }
                else
                {
                    roles = await _context.Roles.Skip(skip).Take(take).OrderByDescending(x => x.Rol_Id).ToListAsync();
                }

                await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, roles);//GUARDAR EN CACHE

                if (roles != null)
                {
                    results.Result = roles;
                    results.Count = roles.Count;
                }

                return (false, errores, results);
            }
        }
        public async Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> PostRolesAsync(Roles_Request_Post value)
        {
            Roles_Response? results = new();
            List<Roles> roles = [];
            List<ErrorServices> errores = [];

            errores = await _roles_Error_Manager.Roles_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_roles = new Roles
            {
                Name = value.Name ?? "",
                Description = value.Description ?? "",
                Date_Insert = currentDateUtc,
                Date_Update = currentDateUtc,
            };

            _context.Roles.Add(new_roles);

            await _context.SaveChangesAsync();

            roles = await _context.Roles.Where(x => x.Name == value.Name).ToListAsync();

            if (roles != null)
            {
                results.Result = roles;
                results.Count = roles.Count();

                roles = await _context.Roles.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListRoles_0_0", roles);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> PatchRolesAsync(Roles_Request_Patch value)
        {
            Roles_Response? results = new();
            List<Roles> roles = [];
            List<ErrorServices> errores = [];

            errores = await _roles_Error_Manager.Roles_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var roless = await _context.Roles.FirstOrDefaultAsync(x => x.Rol_Id == value.Rol_Id);

            if (roless != null)
            {
                DateTime currentDateUtc = DateTime.UtcNow;

                roless.Name = value.Name ?? roless.Name;
                roless.Description = value.Description ?? roless.Description;
                roless.Date_Update = currentDateUtc;

                await _context.SaveChangesAsync();
            }

            roles = await _context.Roles.Where(x => x.Rol_Id == value.Rol_Id).ToListAsync();

            if (roles != null)
            {
                results.Result = roles;
                results.Count = roles.Count;

                roles = await _context.Roles.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListRoles_0_0", roles);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Roles_Response? result)> DeleteRolesAsync(Roles_Request_Delete value)
        {
            Roles_Response? results = new();
            List<Roles> roles = [];
            List<ErrorServices> errores = [];

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var roless = await _context.Roles.FirstOrDefaultAsync(e => e.Rol_Id == value.Rol_Id);

            if (roless == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Rol Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Roles.Remove(roless);

            await _context.SaveChangesAsync();

            if (roles != null)
            {
                results.Result = roles;
                results.Count = roles.Count;

                roles = await _context.Roles.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListRoles_0_0", roles);
            }

            return (false, errores, results);
        }
    }
}
