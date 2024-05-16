using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Pags;
using Manager_Security_BackEnd.Services.Application_Services;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Pages_Services
{
    public class PagesServices : IPages
    {
        private readonly conectionDBcontext _context;
        private readonly Pages_Error_Manager _pages_Error_Manager;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public PagesServices(conectionDBcontext context, Pages_Error_Manager pages_Error_Manager, General_Generate_Cache_Key general_Generate_Cache, IError errorService)
        {
            _context = context;
            _pages_Error_Manager = pages_Error_Manager;
            _generate_Cache_Key = general_Generate_Cache;
            _errorService = errorService;
        }
        public async Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> GetPagesAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Pages_Response? results = new();
            List<Pages>? pages = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListPages_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            pages = await _generate_Cache_Key.Buscar_En_CacheAsync<Pages>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (pages?.Count > 0)
            {
                results.Result = pages?.OrderByDescending(x => x.Page_Id).ToList();
                results.Count = pages?.Count;

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
                    pages = await _context.Pages.Include(x => x.Application).Include(x => x.Application.Company).Where(x => x.Page_Id == value.Id && x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Page_Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    pages = await _context.Pages.Include(x => x.Application).Include(x => x.Application.Company).Where(x => x.Page_Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    pages = await _context.Pages.Include(x => x.Application).Include(x => x.Application.Company).Where(x => x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Page_Id).ToListAsync();
                }
                else
                {
                    pages = await _context.Pages.Include(x => x.Application).Include(x => x.Application.Company).Skip(skip).Take(take).OrderByDescending(x => x.Page_Id).ToListAsync();
                }

                await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, pages);

                if (pages != null)
                {
                    results.Result = pages;
                    results.Count = pages.Count;
                }

                return (false, errores, results);
            }
        }
        public async Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> PostPagesAsync(Pages_Request_Post value)
        {
            Pages_Response? results = new();
            List<Pages> pages = new();
            List<ErrorServices> errores = new();

            errores = await _pages_Error_Manager.Pages_User_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_page = new Pages
            {
                Name = value.Name ?? "",
                Description = value.Description ?? "",
                Date_Insert = currentDateUtc,
                Date_Update = currentDateUtc,
                Application_Id = value.Application_Id,
            };

            _context.Pages.Add(new_page);

            await _context.SaveChangesAsync();

            pages = await _context.Pages.Include(x => x.Application).Where(x => x.Name == value.Name).ToListAsync();

            if (pages != null)
            {
                results.Result = pages;

                results.Count = pages.Count;

                pages = await _context.Pages.Include(x => x.Application).Include(x => x.Application.Company).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListPages_0_0", pages);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> PatchPagesAsync(Pages_Request_Patch value)
        {
            Pages_Response? results = new();
            List<Pages> pages = new();
            List<ErrorServices> errores = new();

            errores = await _pages_Error_Manager.Pages_User_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var page = await _context.Pages.FirstOrDefaultAsync(x => x.Page_Id == value.Page_Id);

            if (page != null)
            {
                DateTime currentDateUtc = DateTime.UtcNow;

                page.Name = value.Name ?? page.Name;
                page.Description = value.Description ?? page.Description;
                page.Date_Update = currentDateUtc;

                await _context.SaveChangesAsync();
            }

            pages.Add(page);

            if (pages != null)
            {
                results.Result = pages;
                results.Count = pages.Count;

                pages = await _context.Pages.Include(x => x.Application).Include(x => x.Application.Company).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListPages_0_0", pages);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Pages_Response? result)> DeletePagesAsync(Pages_Request_Delete value)
        {
            Pages_Response? results = new();
            List<Pages> pages = new();
            List<ErrorServices> errores = new();

            var page = await _context.Pages.FirstOrDefaultAsync(e => e.Page_Id == value.Page_Id);

            if (page == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Page Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Pages.Remove(page);

            await _context.SaveChangesAsync();

            if (pages != null)
            {
                results.Result = pages;
                results.Count = pages.Count;

                pages = await _context.Pages.Include(x => x.Application).Include(x => x.Application.Company).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListPages_0_0", pages);
            }

            return (false, errores, results);
        }
    }
}
