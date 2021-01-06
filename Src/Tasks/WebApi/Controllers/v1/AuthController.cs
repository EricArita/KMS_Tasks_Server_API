using System;
using System.Threading.Tasks;
using MB.Core.Application.Interfaces;
using MB.Core.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MB.WebApi.Models;

namespace MB.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _authService;
        public AuthController(IAuthentication authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
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

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin(FacebookAuthRequest model)
        {
            var result = await _authService.HandleFacebookLoginAsync(model.UserAccessToken);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(response);
        }
    }
}
