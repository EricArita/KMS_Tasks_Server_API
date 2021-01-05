using Core.Application.Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponseModel> GetUserInfoById(long UserId);

        public Task<UserResponseModel> UpdateUserInfo(long updatedByUserId, UpdateUserInfoModel model);
    }
}
