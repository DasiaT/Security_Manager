using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Companys;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Pags;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Company_Services
{ 
    public class CompanyServices: ICompany
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        private readonly Company_Error_Manager _company_Error_Manager;

        public CompanyServices(conectionDBcontext context, IError errorService, Company_Error_Manager company_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _company_Error_Manager = company_Error_Manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Company_Response? result)> GetCompanyAsync([FromQuery] Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Company_Response? results = new();
            List<Company>? companies = new();
            List<ErrorServices> errores = new();
            
            string Key_Value = "ListCompany_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            companies = await _generate_Cache_Key.Buscar_En_CacheAsync<Company>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (companies?.Count > 0)
            {
                results.Result = companies?.OrderByDescending(x => x.Emp_Id).ToList();
                results.Count = companies?.Count;

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
                    companies = await _context.Company.Where(x => x.Emp_Id == value.Id && x.Name.ToLower().Contains(value.Search))
                        .Skip(skip).Take(take).OrderBy(x => x.Emp_Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    companies = await _context.Company.Where(x => x.Emp_Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    companies = await _context.Company.Where(x => x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderBy(x => x.Emp_Id).ToListAsync();
                }
                else
                {
                    companies = await _context.Company.Skip(skip).Take(take).OrderBy(x => x.Emp_Id).ToListAsync();
                }

                await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, companies);//GUARDAR EN CACHE

                if (companies != null)
                {
                    results.Result = companies;
                    results.Count = companies.Count();
                }

                return (false, errores, results);
            }

        }

        public async Task<(bool isError, List<ErrorServices> error, Company_Response? result)> PostCompanyAsync([FromBody] Company_Request_Post value)
        {

            Company_Response? results = new();
            List<Company> companies = new();
            List<ErrorServices> errores = new();

            errores = await _company_Error_Manager.Company_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_company = new Company
            {
                Name = value.Name,
                Description = value.Description ?? "",
                Date_Insert = currentDateUtc,
                Date_Update = currentDateUtc
            };

            _context.Company.Add(new_company);

            await _context.SaveChangesAsync();

            companies = await _context.Company.Where(x => x.Name == value.Name).ToListAsync();

            if (companies != null)
            {
                results.Result = companies;
                results.Count = companies.Count();

                companies = await _context.Company.ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListCompany_0_0", companies);
            }

            return (false, errores, results);

        }

        public async Task<(bool isError, List<ErrorServices> error, Company_Response? result)> PatchCompanyAsync([FromBody] Company_Request_Patch value)
        {
           
            Company_Response? results = new();
            List<Company> companies = new();
            List<ErrorServices> errores = new();

            errores = await _company_Error_Manager.Company_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var companys = await _context.Company.FirstOrDefaultAsync(x => x.Emp_Id == value.Emp_Id);

            if (companys != null)
            {
                DateTime currentDateUtc = DateTime.UtcNow;

                companys.Name = value.Name;
                companys.Description = value.Description ?? companys.Description;
                companys.Date_Update = currentDateUtc;

                await _context.SaveChangesAsync();
            }

            companies = await _context.Company.Where(x => x.Emp_Id == value.Emp_Id).ToListAsync();

            if (companies != null)
            {
                results.Result = companies;
                results.Count = companies.Count();

                companies = await _context.Company.ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListCompany_0_0", companies);
            }

            return (false, errores, results);

        }
        public async Task<(bool isError, List<ErrorServices> error, Company_Response? result)> DeleteCompanyAsync(Company_Request_Delete value)
        {
            Company_Response? results = new();
            List<Company> companies = new();
            List<ErrorServices> errores = new();

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var companys = await _context.Company.FirstOrDefaultAsync(e => e.Emp_Id == value.Emp_Id);

            if (companys == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Company Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Company.Remove(companys);

            await _context.SaveChangesAsync();

            if (companies != null)
            {
                results.Result = companies;
                results.Count = companies.Count;

                companies = await _context.Company.ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListCompany_0_0", companies);
            }

            return (false, errores, results);
        }
    }
}
