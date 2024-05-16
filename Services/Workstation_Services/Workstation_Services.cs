using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Pags;
using Manager_Security_BackEnd.Models.Workstation;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Manager_Security_BackEnd.Services.Workstation_Services
{
    public class WorkstationServices : IWorkstation
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        

        public WorkstationServices(conectionDBcontext context, IError errorService, HttpClient httpClient,IDistributedCache cache, General_Generate_Cache_Key general_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _generate_Cache_Key = general_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> GetWorkstationsAsync([FromForm] Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Information_Workstation_Response? results = new();
            List<Information_Workstation>? workstations = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListWorkstation_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            workstations = await _generate_Cache_Key.Buscar_En_CacheAsync<Information_Workstation>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (workstations?.Count > 0)
            {
                results.Result = workstations?.OrderByDescending(x => x.Id).ToList();
                results.Count = workstations?.Count;

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
                    workstations = await _context.Information_Workstation.Where(x => x.Id == value.Id && x.Name.ToLower().Contains(value.Search))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    workstations = await _context.Information_Workstation.Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    workstations = await _context.Information_Workstation.Where(x => x.Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    workstations = await _context.Information_Workstation
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }

                

                if (workstations != null)
                {
                    results.Result = workstations;
                    results.Count = workstations.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, workstations);
                }

                return (false, errores, results);
            }
        }



        public async Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> PostWorkstationsAsync([FromBody] Information_Workstation_Request value)
        {
            Information_Workstation_Response? results = new();
            List<Information_Workstation> Workstation = [];
            List<ErrorServices> errores = [];

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                ErrorServices _error = _errorService.GetBadRequestException("El campo Nombre no puede ir vacio.", 400);

                return (true, new List<ErrorServices> { _error }, null);
            }

            Workstation = await _context.Information_Workstation.Where(x => x.Name == value.Name).ToListAsync();

            if(Workstation.Count > 0)
            {
                errores.Add(_errorService.GetBadRequestException("No pueden existir dos puestos de trabajo con el mismo nombre.", 400));
            }

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var newWorkstation = new Information_Workstation
            {
                Name = value.Name,
            };

            _context.Information_Workstation.Add(newWorkstation);

            await _context.SaveChangesAsync();

            Workstation = await _context.Information_Workstation.Where(x => x.Name == value.Name).ToListAsync();

            if (Workstation != null)
            {
                results.Result = Workstation;
                results.Count = Workstation.Count();

                // ALMACENAR LOS DATOS EN LA CACHÉ
                Workstation = await _context.Information_Workstation.ToListAsync(); //INSERTAR TODOS LOS DATOS EN CACHE EN LA LISTA

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListWorkstation_0_0", Workstation);
                //FIN DE GUARDAR EN CACHE
            }

            return (false, errores, results);

        }

        public async Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> PatchWorkstationsAsync([FromBody] Information_Workstation_Model value)
        {
            Information_Workstation_Response? results = new();
            List<Information_Workstation> Workstation = new();
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                ErrorServices _error = _errorService.GetBadRequestException("El campo Nombre no puede ir vacio.", 400);

                return (true, new List<ErrorServices> { _error }, null);
            }

            Workstation = await _context.Information_Workstation.Where(x => x.Name == value.Name).ToListAsync();

            if (Workstation.Count > 0)
            {
                errores.Add(_errorService.GetBadRequestException("No pueden existir dos puestos de trabajo con el mismo nombre.", 400));
            }

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var workstation = await _context.Information_Workstation.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (workstation != null)
            {
                workstation.Name = value.Name;

                await _context.SaveChangesAsync();
            }

            Workstation = await _context.Information_Workstation.Where(x => x.Id == value.Id).ToListAsync();

            if (Workstation != null)
            {
                results.Result = Workstation;
                results.Count = Workstation.Count();

                // ALMACENAR LOS DATOS EN LA CACHÉ
                Workstation = await _context.Information_Workstation.ToListAsync(); //INSERTAR TODOS LOS DATOS EN CACHE EN LA LISTA

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListWorkstation_0_0", Workstation);
                //FIN DE GUARDAR EN CACHE
            }

            return (false, errores, results);

        }
        public async Task<(bool isError, List<ErrorServices> error, Information_Workstation_Response? result)> DeleteWorkstationsAsync(Information_Workstation_Request_Delete value)
        {
            Information_Workstation_Response? results = new();
            List<Information_Workstation> information_Workstations = new();
            List<ErrorServices> errores = new();

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var information_workstation = await _context.Information_Workstation.FirstOrDefaultAsync(e => e.Id == value.Id);

            if (information_workstation == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Workstation Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Information_Workstation.Remove(information_workstation);

            await _context.SaveChangesAsync();

            if (information_Workstations != null)
            {
                results.Result = information_Workstations;
                results.Count = information_Workstations.Count;

                // ALMACENAR LOS DATOS EN LA CACHÉ
                information_Workstations = await _context.Information_Workstation.ToListAsync(); //INSERTAR TODOS LOS DATOS EN CACHE EN LA LISTA

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListWorkstation_0_0", information_Workstations);
                //FIN DE GUARDAR EN CACHE
            }

            return (false, errores, results);
        }
    }
}
