using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UM.Core.Application.Interfaces;
using UM.Core.Application.Models;
using UM.Core.Domain.DbEntities;

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

        [HttpGet("getuser/{username}")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            var result = await _userManagementService.GetUserByUsername(username);
            return Ok(result);
        }

        [HttpPost("get-user-role")]
        public async Task<IActionResult> GetUserRole(ApplicationUser user)
        {
            var result = await _userManagementService.GetUserRoleAsync(user);
            return Ok(result);
        }

        [HttpPost("get-user-claim")]
        public async Task<IActionResult> GetUserClaim(ApplicationUser user)
        {
            var result = await _userManagementService.GetUserClaimAsync(user);
            return Ok(result);
        }
    }
}
