using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Pags;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Manager_Security_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : Controller
    {
        private readonly IPages _pages;
        private readonly General_Valid_Token _general_Valid_Token;
        public PagesController(IPages pages, General_Valid_Token general_Valid_Token)
        {
            _pages = pages;
            _general_Valid_Token = general_Valid_Token;
        }

        [HttpGet]
        public async Task<IActionResult> GetPages([FromQuery] Comun_Filters value)
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

                var result = await _pages.GetPagesAsync(value);

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
        public async Task<IActionResult> PostPages([FromBody] Pages_Request_Post value)
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

                var result = await _pages.PostPagesAsync(value);

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
        public async Task<IActionResult> PatchPages([FromBody] Pages_Request_Patch value)
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

                var result = await _pages.PatchPagesAsync(value);

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
        public async Task<IActionResult> DeletePages([FromBody] Pages_Request_Delete value)
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

                var result = await _pages.DeletePagesAsync(value);

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
