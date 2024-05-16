using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Access_Page_Rols;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Access_Page_Rols
{
    public class AccessPageRolServices : IAccess_Page_Rol
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly Access_Page_Rol_Error_Manager _access_Page_Rol_Error_Manager;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public AccessPageRolServices(conectionDBcontext context, IError errorService, Access_Page_Rol_Error_Manager access_Page_Rol_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _access_Page_Rol_Error_Manager = access_Page_Rol_Error_Manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> GetAccessPageRolAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Access_Page_Rol_Response? results = new();
            List<Access_Page_Rol>? access_Page_Rol = [];
            List<ErrorServices> errores = [];

            string Key_Value = "ListAccess_Page_Rol_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            access_Page_Rol = await _generate_Cache_Key.Buscar_En_CacheAsync<Access_Page_Rol>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (access_Page_Rol?.Count > 0)
            {
                results.Result = access_Page_Rol?.OrderByDescending(x => x.Application_Id).ToList();
                results.Count = access_Page_Rol?.Count;

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
                    access_Page_Rol = await _context.Access_Page_Rol
                        .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages)
                        .Where(x => x.Id == value.Id && x.Application.Application_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    access_Page_Rol = await _context.Access_Page_Rol
                        .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages).Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    access_Page_Rol = await _context.Access_Page_Rol.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages)
                        .Where(x => x.Application.Application_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    access_Page_Rol = await _context.Access_Page_Rol.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages).Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }

                if (access_Page_Rol != null)
                {
                    results.Result = access_Page_Rol;
                    results.Count = access_Page_Rol.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, access_Page_Rol);
                }

                return (false, errores, results);
            }
        }

        public async Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> PostAccessPageRolAsync(Access_Page_Rol_Request_Post value)
        {
            Access_Page_Rol_Response? results = new();
            List<Access_Page_Rol> access_Page_Rol = [];
            List<ErrorServices> errores = [];

            errores = await _access_Page_Rol_Error_Manager.Access_Page_Rol_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_access_page_rol = new Access_Page_Rol
            {
                Rol_Id = value.Rol_Id,
                Page_Id = value.Page_Id,
                Application_Id = value.Application_Id,
                Date_Insert = currentDateUtc,
            };

            _context.Access_Page_Rol.Add(new_access_page_rol);

            await _context.SaveChangesAsync();

            access_Page_Rol = await _context.Access_Page_Rol.Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages)
                .Where(x => x.Application_Id == value.Application_Id && x.Page_Id == value.Page_Id && x.Rol_Id == value.Rol_Id).ToListAsync();

            if (access_Page_Rol != null)
            {
                results.Result = access_Page_Rol;
                results.Count = access_Page_Rol.Count;

                access_Page_Rol = await _context.Access_Page_Rol
                    .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListAccess_Page_Rol_0_0", access_Page_Rol);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> PatchAccessPageRolAsync(Access_Page_Rol_Request_Patch value)
        {
            Access_Page_Rol_Response? results = new();
            List<Access_Page_Rol> access_Page_Rol = new();
            List<ErrorServices> errores = new();

            errores = await _access_Page_Rol_Error_Manager.Access_Page_Rol_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var access_Page_Rols = await _context.Access_Page_Rol.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (access_Page_Rols == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Rol Pages Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            access_Page_Rols.Application_Id = value.Application_Id == 0 ? access_Page_Rols.Application_Id : value.Application_Id;
            access_Page_Rols.Rol_Id = value.Rol_Id == 0 ? access_Page_Rols.Rol_Id : value.Application_Id;
            access_Page_Rols.Page_Id = value.Page_Id == 0 ? access_Page_Rols.Page_Id : value.Page_Id;
            access_Page_Rols.Date_Update = currentDateUtc;

            await _context.SaveChangesAsync();

            access_Page_Rol = await _context.Access_Page_Rol
                .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages)
                .Where(x => x.Application_Id == value.Application_Id && x.Rol_Id == value.Rol_Id && x.Page_Id == value.Page_Id).ToListAsync();

            if (access_Page_Rol != null)
            {
                results.Result = access_Page_Rol;
                results.Count = access_Page_Rol.Count;

                access_Page_Rol = await _context.Access_Page_Rol
                    .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListAccess_Page_Rol_0_0", access_Page_Rol);
            }

            return (false, errores, results);
        }

        public async Task<(bool isError, List<ErrorServices> error, Access_Page_Rol_Response? result)> DeleteAccessPageRolAsync(Access_Page_Rol_Request_Delete value)
        {
            Access_Page_Rol_Response? results = new();
            List<Access_Page_Rol> access_Page_Rol = [];
            List<ErrorServices> errores = [];

            var access_Page_Rols = await _context.Access_Page_Rol.FirstOrDefaultAsync(e => e.Id == value.Id);

            if (access_Page_Rols == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Access Rol Pages Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Access_Page_Rol.Remove(access_Page_Rols);

            await _context.SaveChangesAsync();

            if (access_Page_Rols != null)
            {
                results.Result = access_Page_Rol;
                results.Count = access_Page_Rol.Count;

                access_Page_Rol = await _context.Access_Page_Rol
                    .Include(x => x.Application).Include(x => x.Roles).Include(x => x.Pages).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListAccess_Page_Rol_0_0", access_Page_Rol);
            }

            return (false, errores, results);
        }
    }
}
