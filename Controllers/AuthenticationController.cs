using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Manager_Security_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthentication _authentication;
        private readonly General_Valid_Token _general_Valid_Token;
        public AuthenticationController(IAuthentication authentication, General_Valid_Token general_Valid_Token)
        {
            _authentication = authentication;
            _general_Valid_Token = general_Valid_Token;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplication([FromQuery] Comun_Filters value)
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

                var result = await _authentication.GetAuthenticationAsync(value);

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
