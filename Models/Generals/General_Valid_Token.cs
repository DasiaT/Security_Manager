using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Models.Authentications;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.Text;

namespace Manager_Security_BackEnd.Models.Generals
{
    public class General_Valid_Token
    {
        private readonly conectionDBcontext _context;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public General_Valid_Token(conectionDBcontext context, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<bool> GeneralValidTokenURLAsync(string authorizationHeader)
        {
            string token = authorizationHeader.Replace("Bearer ", "");

            //var valida_Token_Cache = await _generate_Cache_Key.Buscar_En_CacheLoginAsync(token);//VERIFICA SI ESTA EN CACHE

            //if (valida_Token_Cache)
            //{
            //    return false;
            //}
            //else
            //{
                var validar_Token_Exist = _context.Authentication.FirstOrDefault(x => x.Token == token && x.Date_Expires_Token > DateTime.UtcNow);

                if (validar_Token_Exist != null)
                {
                    await _generate_Cache_Key.Almacenar_En_CacheLoginAsync(token);
                }

                return validar_Token_Exist == null;
            //}
        }
    }
}
