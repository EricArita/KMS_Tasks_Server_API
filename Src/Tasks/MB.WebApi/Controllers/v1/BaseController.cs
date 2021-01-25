using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace MB.WebApi.Controllers.v1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[area]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected BaseController() { }
        protected long? GetUserId(ClaimsPrincipal claimsManager)
        {
            if (claimsManager == null) return null;
            if (!claimsManager.HasClaim(c => c.Type == "uid"))
            {
                throw new Exception("Token provided is invalid because there is no valid confidential claim");
            }
            // Extract uid from token
            long uid;
            try
            {
                uid = long.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
            }
            catch (Exception)
            {
                throw new Exception("Token provided is invalid because the value for the claim is invalid");
            }
            return uid;
        }
    }
}
