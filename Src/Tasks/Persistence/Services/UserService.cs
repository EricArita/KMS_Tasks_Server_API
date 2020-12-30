using Core.Application.Interfaces;
using Core.Application.Models.User;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class UserService : IUserService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected ILogger<UserService> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        public Task<UserResponseModel> GetUserInfoById(long UserId)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserResponseModel> UpdateUserInfo(long updatedByUserId, UpdateUserInfoModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
