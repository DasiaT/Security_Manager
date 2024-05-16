using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Application_Services
{
    public class ApplicationServices : IApplication
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly Application_Error_Manager _application_Error_Manager;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public ApplicationServices(conectionDBcontext context, IError errorService, Application_Error_Manager application_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _application_Error_Manager = application_Error_Manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Application_Response? result)> GetApplicationAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Application_Response? results = new();
            List<Application>? application = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListApplication_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            application = await _generate_Cache_Key.Buscar_En_CacheAsync<Application>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (application?.Count > 0)
            {
                results.Result = application?.OrderByDescending(x => x.Application_Id).ToList();
                results.Count = application?.Count;

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
                    application = await _context.Application.Include(x => x.Company).Where(x => x.Emp_Id == value.Id && x.Application_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Emp_Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    application = await _context.Application.Include(x => x.Company).Where(x => x.Emp_Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    application = await _context.Application.Include(x => x.Company).Where(x => x.Application_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Emp_Id).ToListAsync();
                }
                else
                {
                    application = await _context.Application.Include(x => x.Company).Skip(skip).Take(take).OrderByDescending(x => x.Emp_Id).ToListAsync();
                }

                if (application != null)
                {
                    results.Result = application;
                    results.Count = application.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, application);
                }

                return (false, errores, results);
            }
        }
        public async Task<(bool isError, List<ErrorServices> error, Application_Response? result)> PostApplicationAsync(Application_Request_Post value)
        {
            Application_Response? results = new();
            List<Application> application = new();
            List<ErrorServices> errores = new();

            errores = await _application_Error_Manager.Application_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_application = new Application
            {
                Application_Name = value.Application_Name ?? "",
                URL_Server = value.URL_Server ?? "",  
                Description = value.Description ?? "",
                Date_Insert = currentDateUtc,
                Date_Update = currentDateUtc,
                Emp_Id = value.Emp_Id ?? 0
            };

            _context.Application.Add(new_application);

            await _context.SaveChangesAsync();

            application = await _context.Application.Include(x => x.Company).Where(x => x.Application_Name == value.Application_Name).ToListAsync();

            if (application != null)
            {
                results.Result = application;
                results.Count = application.Count;

                application = await _context.Application.Include(x => x.Company).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListApplication_0_0", application);
            }

            return (false, errores, results);
        }

        public async Task<(bool isError, List<ErrorServices> error, Application_Response? result)> PatchApplicationAsync(Application_Request_Patch value)
        {
            Application_Response? results = new();
            List<Application> application = new();
            List<ErrorServices> errores = new();

            errores = await _application_Error_Manager.Application_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var applications = await _context.Application.FirstOrDefaultAsync(x => x.Application_Id == value.Application_Id);

            if (applications != null)
            {
                DateTime currentDateUtc = DateTime.UtcNow;

                applications.Application_Name = value.Application_Name ?? applications.Application_Name;
                applications.URL_Server = value.URL_Server ?? applications.URL_Server;
                applications.Description = value.Description ?? applications.Description;
                applications.Date_Update = currentDateUtc;

                await _context.SaveChangesAsync();
            }

            application = await _context.Application.Where(x => x.Application_Id == value.Application_Id).ToListAsync();

            if (application != null)
            {
                results.Result = application;
                results.Count = application.Count;

                application = await _context.Application.Include(x => x.Company).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListApplication_0_0", application);
            }

            return (false, errores, results);

        }

        public async Task<(bool isError, List<ErrorServices> error, Application_Response? result)> DeleteApplicationAsync(Application_Request_Delete value)
        {
            Application_Response? results = new();
            List<Application> application = new();
            List<ErrorServices> errores = new();

            errores = _application_Error_Manager.Application_Valid_Delete(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var applications = await _context.Application.FirstOrDefaultAsync(e => e.Application_Id == value.Application_Id);

            if (applications == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id not exists, insert a valid.", 400));
                
                return (true, errores, null); 
            }

            _context.Application.Remove(applications);

            await _context.SaveChangesAsync();

            if (application != null)
            {
                results.Result = application;
                results.Count = application.Count;

                application = await _context.Application.Include(x => x.Company).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListApplication_0_0", application);
            }

            return (false, errores, results);

        }

    }
}
