using AuthServer.Interfaces;
using AuthServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthServer.Controllers.v1
{
    public class AuthController : BaseController
    {
        private readonly IAuthentication _authService;
        public AuthController(IAuthentication authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequestModel model)
        {
            var result = await _authService.VerifyAccount(model.Username, model.Password);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(5),
            };

            if (result.OK)
            {
                Response.Cookies.Append("refreshToken", result.Data.RefreshToken, cookieOptions);
            }
            return Ok(result);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(response);
        }
    }
}
