using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Users_Applications_Privileges;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Manager_Security_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApplicationPrivilegesController : Controller
    {
        private readonly IUser_Application_Privileges _user_Application_Privileges;
        private readonly General_Valid_Token _general_Valid_Token;
        public UserApplicationPrivilegesController(IUser_Application_Privileges user_Application_Privileges, General_Valid_Token general_Valid_Token)
        {
            _user_Application_Privileges = user_Application_Privileges;
            _general_Valid_Token = general_Valid_Token;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserApplicationPrivileges([FromQuery] Comun_Filters value)
        {

            var authorizationHeader = HttpContext.Request.Headers.Authorization;

            var valid_Token = await _general_Valid_Token.GeneralValidTokenURLAsync(authorizationHeader.ToString());

            if (valid_Token)
            {
                var errorDetails = new { message = "Invalid token user,  you must log in first.", error_code = 403, status_code = 403 };

                return StatusCode(403, errorDetails);
            }

            var result = await _user_Application_Privileges.GetUserApplicationPrivilegesAsync(value);

            if (result.isError)
            {
                var ex = result.error;

                List<object> errorDetails = new List<object>();

                foreach (var error in ex)
                {
                    errorDetails.Add(new { message = error.Message, error_code = error.ErrorCode, status_code = error.StatusCode });
                }

                return StatusCode((int)HttpStatusCode.BadRequest, errorDetails);

            }

            return Ok(result.result);

        }

        [HttpPost]
        public async Task<IActionResult> PostUserApplicationPrivileges([FromBody] User_Application_Privileges_Request_Post value)
        {
            try
            {
                var authorizationHeader = HttpContext.Request.Headers.Authorization;

                var valid_Token = await _general_Valid_Token.GeneralValidTokenURLAsync(authorizationHeader.ToString());

                if (valid_Token)
                {
                    var errorDetails = new { message = "Invalid token user,  you must log in first.", error_code = 403, status_code = 403 };
                    return StatusCode(403, errorDetails);
                }

                var result = await _user_Application_Privileges.PostUserApplicationPrivilegesAsync(value);

                if (result.isError)
                {
                    var ex = result.error;

                    List<object> errorDetails = new List<object>();

                    foreach (var error in ex)
                    {
                        errorDetails.Add(new { message = error.Message, error_code = error.ErrorCode, status_code = error.StatusCode });
                    }

                    return StatusCode((int)HttpStatusCode.BadRequest, errorDetails);

                }

                return Ok(result.result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    error_code = 5000,
                    status_code = 500
                });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUserApplicationPrivileges([FromBody] User_Application_Privileges_Request_Patch value)
        {
            try
            {
                var authorizationHeader = HttpContext.Request.Headers.Authorization;

                var valid_Token = await _general_Valid_Token.GeneralValidTokenURLAsync(authorizationHeader.ToString());

                if (valid_Token)
                {
                    var errorDetails = new { message = "Invalid token user,  you must log in first.", error_code = 403, status_code = 403 };
                    return StatusCode(403, errorDetails);
                }

                var result = await _user_Application_Privileges.PatchUserApplicationPrivilegesAsync(value);

                if (result.isError)
                {
                    var ex = result.error;

                    List<object> errorDetails = new List<object>();

                    foreach (var error in ex)
                    {
                        errorDetails.Add(new { message = error.Message, error_code = error.ErrorCode, status_code = error.StatusCode });
                    }

                    return StatusCode((int)HttpStatusCode.BadRequest, errorDetails);

                }

                return Ok(result.result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    error_code = 5000,
                    status_code = 500
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserApplicationPrivileges([FromBody] User_Application_Privileges_Request_Delete value)
        {
            try
            {
                var authorizationHeader = HttpContext.Request.Headers.Authorization;

                var valid_Token = await _general_Valid_Token.GeneralValidTokenURLAsync(authorizationHeader.ToString());

                if (valid_Token)
                {
                    var errorDetails = new { message = "Invalid token user,  you must log in first.", error_code = 403, status_code = 403 };

                    return StatusCode(403, errorDetails);
                }

                var result = await _user_Application_Privileges.DeleteUserApplicationPrivilegesAsync(value);

                if (result.isError)
                {
                    var ex = result.error;

                    List<object> errorDetails = new List<object>();

                    foreach (var error in ex)
                    {
                        errorDetails.Add(new { message = error.Message, error_code = error.ErrorCode, status_code = error.StatusCode });
                    }

                    return StatusCode((int)HttpStatusCode.BadRequest, errorDetails);

                }

                return Ok(result.result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    error_code = 5000,
                    status_code = 500
                });
            }
        }
    }
}
