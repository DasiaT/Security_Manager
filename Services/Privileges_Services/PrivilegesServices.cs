using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Privilegs;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Privileges_Services
{
    public class PrivilegesServices : IPrivileges
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        private readonly Privileges_Error_Manager _privileges_Error_Manager;
        public PrivilegesServices(conectionDBcontext context, IError errorService, Privileges_Error_Manager privileges_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _privileges_Error_Manager = privileges_Error_Manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
       
        public async Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> GetPrivilegesAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Privileges_Response? results = new();
            List<Privileges>? privileges = [];
            List<ErrorServices> errores = [];

            string Key_Value = "ListPrivileges_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            privileges = await _generate_Cache_Key.Buscar_En_CacheAsync<Privileges>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (privileges?.Count > 0)
            {
                results.Result = privileges?.OrderByDescending(x => x.Id).ToList();
                results.Count = privileges?.Count;

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
                    privileges = await _context.Privileges.Where(x => x.Id == value.Id && x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).ToListAsync();
                }
                else if (value.Id != null)
                {
                    privileges = await _context.Privileges.Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    privileges = await _context.Privileges.Where(x => x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    privileges = await _context.Privileges.Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }

                if (privileges != null)
                {
                    results.Result = privileges;
                    results.Count = privileges.Count;

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, privileges);//GUARDAR EN CACHE
                }

                return (false, errores, results);
            }
        }
        public async Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> PostPrivilegesAsync(Privileges_Request_Post value)
        {
            Privileges_Response? results = new();
            List<Privileges> privileges = [];
            List<ErrorServices> errores = [];

            errores = await _privileges_Error_Manager.Privileges_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_privileges = new Privileges
            {
                Name = value.Name ?? "",
                Description = value.Description ?? "",
                Date_Insert = currentDateUtc
            };

            _context.Privileges.Add(new_privileges);

            await _context.SaveChangesAsync();

            privileges = await _context.Privileges.Where(x => x.Name == value.Name).ToListAsync();

            if (privileges != null)
            {
                results.Result = privileges;
                results.Count = privileges.Count();

                privileges = await _context.Privileges.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListPrivileges_0_0", privileges);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> PatchPrivilegesAsync(Privileges_Request_Patch value)
        {
            Privileges_Response? results = new();
            List<Privileges> privileges = [];
            List<ErrorServices> errores = [];

            errores = await _privileges_Error_Manager.Privileges_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var privilegs = await _context.Privileges.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (privilegs != null)
            {
                DateTime currentDateUtc = DateTime.UtcNow;

                privilegs.Name = value.Name ?? privilegs.Name;
                privilegs.Description = value.Description ?? privilegs.Description;
                privilegs.Date_Update = currentDateUtc;

                await _context.SaveChangesAsync();
            }

            privileges = await _context.Privileges.Where(x => x.Id == value.Id).ToListAsync();

            if (privileges != null)
            {
                results.Result = privileges;
                results.Count = privileges.Count;

                privileges = await _context.Privileges.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListPrivileges_0_0", privileges);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Privileges_Response? result)> DeletePrivilegesAsync(Privileges_Request_Delete value)
        {
            Privileges_Response? results = new();
            List<Privileges> privileges = [];
            List<ErrorServices> errores = [];

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var privilegs = await _context.Privileges.FirstOrDefaultAsync(e => e.Id == value.Id);

            if (privilegs == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Privileges Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Privileges.Remove(privilegs);

            await _context.SaveChangesAsync();

            if (privileges != null)
            {
                results.Result = privileges;
                results.Count = privileges.Count;

                privileges = await _context.Privileges.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListPrivileges_0_0", privileges);
            }

            return (false, errores, results);
        }
    }
}
