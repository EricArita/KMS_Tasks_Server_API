using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.User;
using MB.Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System;
using MB.Core.Domain.Constants;
using MB.Core.Application.Helper.Exceptions.User;

namespace MB.Infrastructure.Services.Internal
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

        public async Task<UserResponseModel> GetUserInfoById(long UserId)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                var validUser = _userManager.Users.FirstOrDefault(user => user.UserId == UserId);
                if (validUser == null)
                {
                    throw new UserServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                await transaction.CommitAsync();

                return new UserResponseModel(validUser);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.USER_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<UserResponseModel> UpdateUserInfo(long updatedByUserId, UpdateUserInfoModel model)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                var validUser = _userManager.Users.FirstOrDefault(user => user.UserId == updatedByUserId);
                if(validUser == null)
                {
                    throw new UserServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                bool isUpdated = false;
                if(model.FirstName != null)
                {
                    validUser.FirstName = model.FirstName;
                    isUpdated = true;
                }

                if(model.LastName != null)
                {
                    validUser.LastName = model.LastName;
                    isUpdated = true;
                }

                if(model.MidName != null)
                {
                    validUser.MidName = model.MidName;
                    isUpdated = true;
                }

                if(model.Phone != null)
                {
                    validUser.PhoneNumber = model.Phone;
                    isUpdated = true;
                }

                if(model.AvatarUrl != null)
                {
                    validUser.AvatarUrl = model.AvatarUrl;
                    isUpdated = true;
                }

                if(model.Address != null)
                {
                    validUser.Address = model.Address;
                    isUpdated = true;
                }

                if(model.NewPassword != null)
                {
                    if(model.CurrentPassword == null)
                    {
                        throw new UserServiceException(UserRelatedErrorsConstants.MISSING_CURRENT_PASSWORD_WHEN_CHANGING_PASSWORD);
                    }
                    var res = (await _userManager.ChangePasswordAsync(validUser, model.CurrentPassword, model.NewPassword));
                    if(!res.Succeeded)
                    {
                        throw new UserServiceException(UserRelatedErrorsConstants.PASSWORD_CHANGE_ERROR, res.Errors.ToList());
                    }
                    isUpdated = true;
                }

                DateTime rightNow = DateTime.UtcNow;

                if (isUpdated)
                {
                    validUser.UpdatedDate = rightNow;
                    await _userManager.UpdateAsync(validUser);
                    await _unitOfWork.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return new UserResponseModel(validUser);
            } catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.USER_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }
    }
}
