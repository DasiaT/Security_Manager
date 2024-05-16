using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Authentications;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Users;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Manager_Security_BackEnd.Services.User_Services
{
    public class UserServices : IUser
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        private readonly General_Generate_Token _general_Generate_Token;
        private readonly User_Error_Manager _user_Error_Manager;
        private readonly General_Generate_Password _general_Generate_Password;
        public UserServices(conectionDBcontext context, IError errorService, General_Generate_Token general_Generate_Token, General_Generate_Cache_Key generate_Cache_Key, User_Error_Manager user_Error_Manager, General_Generate_Password general_Generate_Password)
        {
            _context = context;
            _errorService = errorService;
            _generate_Cache_Key = generate_Cache_Key;
            _general_Generate_Token = general_Generate_Token;
            _user_Error_Manager = user_Error_Manager;
            _general_Generate_Password = general_Generate_Password;
        }

        public async Task<(bool isError, List<ErrorServices> error, User_Response? result)> GetUserAsync(Comun_Filters value)
        {

            int take = 15;//VALORES INICIALES POR SI VAN VACIOS DE LIMIT
            int skip = 0;//VALORES INICIALES POR SI VAN VACIOS DE OFFSET

            User_Response? results = new();
            List<User>? users = new();
            List<ErrorServices> errores = new();

            string Key_Value = "ListUsers_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            users = await _generate_Cache_Key.Buscar_En_CacheAsync<User>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (users?.Count > 0)
            {
                results.Result = users?.OrderByDescending(x => x.User_Id).ToList();
                results.Count = users?.Count;

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
                    users = await _context.User.Include(u => u.Information_User).ThenInclude(x => x.Information_Workstation).Where(x => x.User_Id == value.Id && x.User_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderBy(x => x.Id).ToListAsync();
                }
                else if (value.Id != null)
                {
                    users = await _context.User.Include(u => u.Information_User).ThenInclude(x => x.Information_Workstation).Where(x => x.Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    users = await _context.User.Include(u => u.Information_User).ThenInclude(x => x.Information_Workstation).Where(x => x.User_Name.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderBy(x => x.Id).ToListAsync();
                }
                else
                {
                    users = await _context.User.Include(x => x.Information_User).ThenInclude(x => x.Information_Workstation).Skip(skip).Take(take).OrderBy(x => x.Id).ToListAsync();
                }

                if (users != null)
                {
                    results.Result = users;
                    results.Count = users.Count();

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, users);
                }

                return (false, errores, results);
            }
        }

        public async Task<(bool isError, List<ErrorServices> error, User_Response_Login? result)> PostUserLoginAsync(User_Request_Post_Login value)
        {
            User_Response_Login? results = new();
            List<User> users = new();
            List<User_Login> user_login = new();
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.User_Name) || string.IsNullOrEmpty(value.Password))
            {
                errores.Add(_errorService.GetNotFoundException($"El usuario o contraseña no puede ir vacio.", 400));
            }

            var usuario = await _context.User.FirstOrDefaultAsync(x => x.User_Name == value.User_Name);

            if (usuario == null)
            {
                errores.Add(_errorService.GetNotFoundException($"Usuario o contraseña no valido.", 404));

                return (true, errores, null);
            }

            //var password_salt = await _general_Generate_Password.Generate_SHA512_Password(usuario.Password);

            var password_md5 = await _general_Generate_Password.ValidatePasswordAsync(value.Password, usuario.Password);

            users = await _context.User.Include(u => u.Information_User).ThenInclude(x => x.Information_Workstation).Where(x => x.User_Name == value.User_Name && x.Password == password_md5).ToListAsync();

            if (users.Count == 0)
            {
                errores.Add(_errorService.GetNotFoundException($"Usuario o contraseña no valido.", 404));

                return (true, errores, null);
            }

            if (users.Count > 0)
            {
                Boolean valid_user = users[0].State;

                if (valid_user == false)
                {
                    errores.Add(_errorService.GetNotFoundException($"Usuario no esta activo.", 404));

                    return (true, errores, null);
                }

                //EMPIEZA A FORMAR EL JSON QUE TENDRA RESPUESTA EN EL LOGIN
                var expiration = DateTime.UtcNow.AddYears(1);//TIEMPO QUE EXPIRA EL TOKEN
                var expires_In_Minutes = (int)(expiration - DateTime.UtcNow).TotalMinutes;

                var new_user_claims = new User_Login
                {
                    Id = users[0].Id,
                    User_Name = users[0].User_Name,
                    User_Id = users[0].User_Id,
                    Information_User = users[0].Information_User,
                    State = users[0].State,
                    ExpiresInMinutes = expires_In_Minutes,
                    //User_Application_Rol =
                };

                var createClaims = await _general_Generate_Token.CreateClaimsAsync(new_user_claims);//DATOS QUE SE GUARDARAN EN EL TOKEN

                var createSing = _general_Generate_Token.CreateSigningCredentials();//FIRMA QUE TENDRA EL TOKEN

                var token = _general_Generate_Token.CreateJwtToken(createClaims, createSing, expiration);//CREACION DEL TOKEN

                var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

                new_user_claims.Token = tokenHandler.ToString();

                user_login.Add(new_user_claims);

                //GUARDAR EN BASE DE DATOS
                var new_authentication = new Authentication
                {
                    Token = tokenHandler.ToString(),
                    Application_Id = 1,
                    User_Id = users[0].User_Id,
                    Date_Expires_Token = expiration,
                    Date_Insert = DateTime.UtcNow,
                    Date_Session = DateTime.UtcNow,
                    Date_Update_Token = DateTime.UtcNow
                };

                _context.Authentication.Add(new_authentication);

                await _context.SaveChangesAsync();
                //FIN DE GUARDAR

                //var validar_Token_Exist = _context.Authentication.Where(x => x.Date_Expires_Token > DateTime.UtcNow).ToList();

                //if (validar_Token_Exist != null)
                //{
                    
                //    foreach (var items in validar_Token_Exist)
                //    {
                //       await _generate_Cache_Key.Almacenar_En_CacheLoginAsync("token" + items.Token); 
                //    }
                //}
            }

            if (user_login != null)
            {
                results.Result = user_login;
                results.Count = users.Count();
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Response? result)> PostUserAsync(User_Request_Post value)
        {
            User_Response? results = new();
            List<User> user = new();
            List<ErrorServices> errores = new();

            errores = await _user_Error_Manager.User_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var password_md5 = await _general_Generate_Password.Generate_SHA512_Password(value.Password);

            var new_user = new User
            {
                User_Name = value.User_Name ?? "",
                Password = password_md5 ?? "",
                State = true,
                Date_Insert = currentDateUtc,
                Date_Update = currentDateUtc,
                User_Id = value.User_Id
            };

            _context.User.Add(new_user);

            await _context.SaveChangesAsync();

            user = await _context.User.Include(x => x.Information_User).Where(x => x.User_Name == value.User_Name).ToListAsync();

            if (user != null)
            {
                results.Result = user;
                results.Count = user.Count;

                user = await _context.User.Include(x => x.Information_User).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUsers_0_0", user);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, User_Response? result)> PatchUserAsync(User_Request_Patch value)
        {
            User_Response? results = new();
            List<User> user = new();
            List<ErrorServices> errores = new();

            var users = await _context.User.FirstOrDefaultAsync(x => x.Id == value.Id);

            if (users == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var password_md5 = await _general_Generate_Password.Generate_SHA512_Password(value.Password ?? "");

            users.User_Name = value.User_Name != null ? value.User_Name : users.User_Name;
            users.Password = password_md5 != null ? password_md5 : users.Password;
            users.State = value.State ?? users.State;
            users.Date_Update = currentDateUtc;
            users.User_Id = value.User_Id ?? users.User_Id;
           
            await _context.SaveChangesAsync();

            user = await _context.User.Include(x => x.Information_User.Information_Workstation).Where(x => x.Id == value.Id).ToListAsync();

            if (user != null)
            {
                results.Result = user;
                results.Count = user.Count;

                user = await _context.User.Include(x => x.Information_User).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUsers_0_0", user);
            }

            return (false, errores, results);

        }
        public async Task<(bool isError, List<ErrorServices> error, User_Response? result)> DeleteUserAsync(User_Request_Delete value)
        {
            User_Response? results = new();
            List<User> user = new();
            List<ErrorServices> errores = new();

            var users = await _context.User.FirstOrDefaultAsync(e => e.User_Id == value.Id);

            if (users == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.User.Remove(users);

            await _context.SaveChangesAsync();

            if (user != null)
            {
                results.Result = user;
                results.Count = user.Count;

                user = await _context.User.Include(x => x.Information_User).ToListAsync();

                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListUsers_0_0", user);
            }

            return (false, errores, results);
        }
    }
}
