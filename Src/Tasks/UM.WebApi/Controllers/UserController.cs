using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UM.Core.Application.Interfaces;
using UM.Core.Application.Models;

namespace UM.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManagement _userManagementService;
        public UserController(IUserManagement authService)
        {
            _userManagementService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            var result = await _userManagementService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpGet("getuser/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userManagementService.GetUserByEmail(email);
            return Ok(result);
        }
    }
}
