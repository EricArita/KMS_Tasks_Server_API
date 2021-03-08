using System;
using System.Threading.Tasks;
using MB.Core.Application.Helper;
using MB.Core.Application.Interfaces;
using MB.Core.Application.Interfaces.Misc;
using MB.Core.Application.Models;
using MB.Core.Domain.DbEntities;
using MB.WebApi.Hubs.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MB.WebApi.Controllers.v1
{
    [Area("authentication")]
    public class AuthController : BaseController
    {
        private readonly IAuthentication _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthentication authService, UserManager<ApplicationUser> userManager , ILogger<AuthController> logger) : base(userManager)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result.OK)
            {
                return Ok(result);
            } else
            {
                return BadRequest(result);
            }
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
                return Ok(result);
            } else
            {
                return BadRequest(result);
            }          
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
            if (response.OK)
            {
                return Ok(response);
            } else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("check-token-valid")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CheckToken()
        {
            return Ok(new HttpResponse<string>(true, null, "Your token is valid", null));
        }
    }
}
