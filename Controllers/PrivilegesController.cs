using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Privilegs;
using Manager_Security_BackEnd.Models.Rols;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Manager_Security_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivilegesController : Controller
    {
        private readonly IPrivileges _privileges;
        private readonly General_Valid_Token _general_Valid_Token;
        public PrivilegesController(IPrivileges privileges, General_Valid_Token general_Valid_Token)
        {
            _privileges = privileges;
            _general_Valid_Token = general_Valid_Token;
        }
        [HttpGet]
        public async Task<IActionResult> GetPrivileges([FromQuery] Comun_Filters value)
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

                var result = await _privileges.GetPrivilegesAsync(value);

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

        [HttpPost]
        public async Task<IActionResult> PostPrivileges([FromBody] Privileges_Request_Post value)
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

                var result = await _privileges.PostPrivilegesAsync(value);

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
        public async Task<IActionResult> PatchPrivileges([FromBody] Privileges_Request_Patch value)
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

                var result = await _privileges.PatchPrivilegesAsync(value);

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
        public async Task<IActionResult> DeletePrivileges([FromBody] Privileges_Request_Delete value)
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

                var result = await _privileges.DeletePrivilegesAsync(value);

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
