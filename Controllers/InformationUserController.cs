﻿using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Information_Users;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Manager_Security_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationUserController : Controller
    {
        private readonly IInformation_User _informationUser;
        private readonly General_Valid_Token _general_Valid_Token;

        public InformationUserController(IInformation_User informationUser, General_Valid_Token general_Valid_Token)
        {
            _informationUser = informationUser;
            _general_Valid_Token = general_Valid_Token;
        }

        [HttpGet]
        public async Task<IActionResult> GetInformationUser([FromQuery] Comun_Filters value)
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

                var result = await _informationUser.GetInformationUserAsync(value);

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
        public async Task<IActionResult> PostInformationUser([FromBody] Information_User_Request value)
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

                var result = await _informationUser.PostInformationUserAsync(value);

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
        public async Task<IActionResult> PatchInformationUser([FromBody] Information_User_Request_Patch value)
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

                var result = await _informationUser.PatchInformationUserAsync(value);

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
        public async Task<IActionResult> DeleteInformationUser([FromBody] Information_User_Request_Delete value)
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

                var result = await _informationUser.DeleteInformationUserAsync(value);

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