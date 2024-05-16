using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Information_Users;
using Manager_Security_BackEnd.Models.User;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Manager_Security_BackEnd.Services.Information_User_Services
{
    public class InformationUserServices : IInformation_User
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        private readonly Information_User_Error_Manager _information_User_Error_Manager;

        public InformationUserServices(conectionDBcontext context, IError errorService, Information_User_Error_Manager information_User_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _information_User_Error_Manager = information_User_Error_Manager;   
            _generate_Cache_Key = generate_Cache_Key;
        }

        public async Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> GetInformationUserAsync([FromForm] Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Information_User_Response? results = new();
            List<Information_User>? information_Users = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListInformationUser_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            information_Users = await _generate_Cache_Key.Buscar_En_CacheAsync<Information_User>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (information_Users?.Count > 0)
            {
                results.Result = information_Users?.OrderByDescending(x => x.Id).ToList();
                results.Count = information_Users?.Count;

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
                    information_Users = await _context.Information_User.Include(u => u.Information_Workstation).Where(x => x.Id == value.Id && x.Name.ToLower().Contains(value.Search) || x.DNI == value.Search)
                        .Skip(skip).Take(take).OrderBy(x => x.Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    information_Users = await _context.Information_User.Include(u => u.Information_Workstation).Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    information_Users = await _context.Information_User.Include(u => u.Information_Workstation).Where(x => x.Name.ToLower().Contains(value.Search.ToLower()) || x.Email.ToLower().Contains(value.Search.ToLower()) || x.DNI.StartsWith(value.Search))
                        .Skip(skip).Take(take).OrderBy(x => x.Id).ToListAsync();
                }
                else
                {
                    information_Users = await _context.Information_User.Include(u => u.Information_Workstation).Skip(skip).Take(take).OrderBy(x => x.Id).ToListAsync();
                }

                if (information_Users != null)
                {
                    results.Result = information_Users;
                    results.Count = information_Users.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, information_Users);//GUARDAR EN CACHE
                }

                return (false, errores, results);
            }

        }

        public async Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> PostInformationUserAsync([FromBody] Information_User_Request value)
        {
            
            Information_User_Response? results = new();
            List<Information_User> information_Users = new();
            List<ErrorServices> errores = new();


            errores = await _information_User_Error_Manager.Information_User_Valid(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_information_user = new Information_User
            {
                Name = value.Name,
                Surnames = value.Surnames,  
                Email = value.Email,
                DNI = value.DNI,
                State = true,
                Description = value.Descripcion ?? "",
                Id_workstation = value.Id_workstation,
                Date_Insert = currentDateUtc,
                Date_Update = currentDateUtc,
            };

            _context.Information_User.Add(new_information_user);

            await _context.SaveChangesAsync();

            information_Users = await _context.Information_User.Where(x => x.Name == value.Name).ToListAsync();

            if (information_Users != null)
            {
                results.Result = information_Users;
                results.Count = information_Users.Count();

                information_Users = await _context.Information_User.Include(x => x.Information_Workstation).ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListInformationUser_0_0", information_Users);
            }

            return (false, errores, results);

        }

        public async Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> PatchInformationUserAsync([FromBody] Information_User_Request_Patch value)
        {
            
            Information_User_Response? results = new();
            List<Information_User> information_Users = new();
            List<ErrorServices> errores = new();


            errores = await _information_User_Error_Manager.Information_User_ValidPatch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var information_User = await _context.Information_User.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (information_User != null)
            {
                DateTime currentDateUtc = DateTime.UtcNow;

                information_User.Name = value.Name;
                information_User.Surnames = value.Surnames;
                information_User.Email = value.Email;
                information_User.State = value.State ?? information_User.State;
                information_User.DNI = value.DNI;
                information_User.Description = value.Descripcion ?? information_User.Description;
                information_User.Id_workstation = value.Id_workstation;
                information_User.Date_Update = currentDateUtc;

                await _context.SaveChangesAsync();
            }

            information_Users = await _context.Information_User.Where(x => x.Id == value.Id).ToListAsync();

            if (information_Users != null)
            {
                results.Result = information_Users;
                results.Count = information_Users.Count();

                information_Users = await _context.Information_User.Include(x => x.Information_Workstation).ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListInformationUser_0_0", information_Users);
            }

            return (false, errores, results);

        }

        public async Task<(bool isError, List<ErrorServices> error, Information_User_Response? result)> DeleteInformationUserAsync(Information_User_Request_Delete value)
        {
            Information_User_Response? results = new();
            List<Information_User> information_Users = new();
            List<ErrorServices> errores = new();

            //errores = _roles_Error_Manager.Roles_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }
            var validarUser = await _context.User.FirstOrDefaultAsync(x => x.User_Id == value.Id);

            if (validarUser != null)
            {
                errores.Add(_errorService.GetBadRequestException("The Id cannot be deleted.", 400));

                return (true, errores, null);
            }

            var informations_Users = await _context.Information_User.FirstOrDefaultAsync(e => e.Id == value.Id);

            if (informations_Users == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Information_User.Remove(informations_Users);

            await _context.SaveChangesAsync();

            if (information_Users != null)
            {
                results.Result = information_Users;
                results.Count = information_Users.Count;

                information_Users = await _context.Information_User.Include(x => x.Information_Workstation).ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListInformationUser_0_0", information_Users);
            }

            return (false, errores, results);
        }
    }
}
