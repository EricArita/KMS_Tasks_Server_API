using System;
using System.Threading.Tasks;
using MB.Core.Application.Interfaces;
using MB.Core.Application.Models;
using MB.Core.Domain.DbEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MB.WebApi.Controllers.v1
{
    [Area("authentication")]
    public class AuthController : BaseController
    {
        private readonly IAuthentication _authService;
        public AuthController(IAuthentication authService, UserManager<ApplicationUser> userManager) : base(userManager)
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
