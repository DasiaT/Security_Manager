using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Models.Authentications;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Application_Services;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Authentications
{
    public class AuthenticationServices : IAuthentication
    {
        private readonly conectionDBcontext _context;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public AuthenticationServices(conectionDBcontext context, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Authentication_Response? result)> GetAuthenticationAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Authentication_Response? results = new();
            List<Authentication>? authentication = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListAuthentication_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            authentication = await _generate_Cache_Key.Buscar_En_CacheAsync<Authentication>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (authentication?.Count > 0)
            {
                results.Result = authentication?.OrderByDescending(x => x.Application_Id).ToList();
                results.Count = authentication?.Count;

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
                    authentication = await _context.Authentication.Include(x => x.Application).Include(x => x.User).Where(x => x.Application_Id == value.Id && x.Application.Application_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    authentication = await _context.Authentication.Include(x => x.Application).Include(x => x.User).Where(x => x.Application_Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    authentication = await _context.Authentication.Include(x => x.Application_Id).Include(x => x.User).Where(x => x.User.User_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    authentication = await _context.Authentication.Include(x => x.Application).Include(x => x.User).Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync();
                }

                await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, authentication);

                if (authentication != null)
                {
                    results.Result = authentication;
                    results.Count = authentication.Count();
                }

                return (false, errores, results);
            }
        }
    }
}
