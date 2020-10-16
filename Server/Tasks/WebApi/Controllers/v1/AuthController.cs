using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Application.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers.v1
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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequestModel model)
        {
            var result = await _authService.VerifyAccount(model.Username, model.Password);
            return Ok(result);
        }
    }
}
