using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Manager_Security_BackEnd.Models.Generals
{
    public class General_Generate_Cache_Key
    {
        private readonly IDistributedCache _cache;
        public General_Generate_Cache_Key(IDistributedCache cache)
        {
            _cache = cache;
        }
        public string General_GenerateCacheKey(Comun_Filters value, string cacheKey)
        {
            StringBuilder cacheKeyBuilder = new StringBuilder(cacheKey);

            if (value.Id != 0)
            {
                cacheKeyBuilder.Append(value.Id);
            }

            if (!string.IsNullOrEmpty(value.Search))
            {
                if (cacheKeyBuilder.Length > cacheKey.Length)
                {
                    cacheKeyBuilder.Append('_');
                }
                cacheKeyBuilder.Append(value.Search);
            }

            if (cacheKeyBuilder.Length > cacheKey.Length)
            {
                cacheKeyBuilder.Append('_');
            }

            cacheKeyBuilder.Append(value.Take).Append('_').Append(value.Skip);

            return cacheKeyBuilder.ToString();
        }

        public string General_GenerateCacheKey_Login(string cacheKey)
        {
            return cacheKey;
        }

        public async Task Almacenar_En_CacheAsync<T>(string cacheKey, T data)
        {
            int expirationCache = 10;

            var serializedData = JsonSerializer.Serialize(data);//SERIALIZAMOS LOS DATOS

            var dataBytes = Encoding.UTF8.GetBytes(serializedData);//CONVERTIR EN ARRAY DE BYTE

            var cacheOptions = new DistributedCacheEntryOptions//CONFIGURACION DE EXPIRACION DE CACHE
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationCache)
            };

            await _cache.SetAsync(cacheKey, dataBytes, cacheOptions); //ALMACENAR EN CACHE
        }

        public async Task Almacenar_En_CacheLoginAsync(string cacheKey)
        {
            int expirationCache = 10;

            var serializedData = JsonSerializer.Serialize(cacheKey);//SERIALIZAMOS LOS DATOS

            var dataBytes = Encoding.UTF8.GetBytes(serializedData);//CONVERTIR EN ARRAY DE BYTE

            var cacheOptions = new DistributedCacheEntryOptions//CONFIGURACION DE EXPIRACION DE CACHE
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationCache)
            };

            await _cache.SetAsync(cacheKey, dataBytes, cacheOptions); //ALMACENAR EN CACHE
        }

        public async Task<List<T>?> Buscar_En_CacheAsync<T>(string cacheKey)
        {
            List<T>? datosCache = [];

            var cacheData = await _cache.GetAsync(cacheKey);//VERIFICA SI ESTA EN CACHE

            if(cacheData != null)
            {
                string serializedKey = Encoding.UTF8.GetString(cacheData);

                datosCache = JsonSerializer.Deserialize<List<T>>(serializedKey);
                
            }
            
            return datosCache;
        }

        public async Task<Boolean> Buscar_En_CacheLoginAsync(string cacheKey)
        {
            Boolean datosCache = false;

            var cacheData = await _cache.GetAsync(cacheKey);//VERIFICA SI ESTA EN CACHE

            if (cacheData != null)
            {
                string serializedKey = Encoding.UTF8.GetString(cacheData);

                JsonSerializer.Deserialize<string>(serializedKey);

                datosCache = true;
            }

            return datosCache;
        }
    }
}
