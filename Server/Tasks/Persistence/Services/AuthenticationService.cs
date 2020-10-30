using Core.Application.Helper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.Constants;
using Core.Domain.Entities;
using Infrastructure.Persistence.SettingModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public abstract class AuthenticationService : GenerateJWTAuthentication, IAuthentication
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<Response<ApplicationUser>> RegisterAsync(RegisterModel model)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            var res = new Response<ApplicationUser>();

            if (userWithSameEmail == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    FirstName = model.FirstName,
                    MidName = model.MidName,
                    LastName = model.LastName,
                    Active = true
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Enums.UserRoles.User.ToString());

                    res.OK = true;
                    res.Message = $"Register successfully";
                    res.Data = newUser;
                }
                else
                {
                    res.OK = false;
                    res.Message = $"Register fail";
                    res.Errors = new List<string>();
                    foreach(var error in result.Errors)
                    {
                        res.Errors.Add(error.Description);
                    }
                }
            }
            else
            {
                res.OK = true;
                res.Message = $"Email or Username {model.Email } is already registered.";
            }

            return res;
        }

        public async Task<Response<AuthResponseModel>> VerifyAccount(string userName, string password)
        {
            var authResponseModel = new AuthResponseModel();
            var res = new Response<AuthResponseModel>();

            var user = await _userManager.FindByNameAsync(userName) ?? await _userManager.FindByEmailAsync(userName);

            if (user == null)
            {
                authResponseModel.IsAuthenticated = false;
                res.OK = false;
                res.Message = $"There is not any accounts registered with {userName}.";
                res.Data = authResponseModel;
            }
            else if (!_userManager.CheckPasswordAsync(user, password).Result)
            {
                authResponseModel.IsAuthenticated = false;
                res.OK = false;
                res.Message = "Password is invalid";
                res.Data = authResponseModel;
            }
            else
            {
                var rolesList = await _userManager.GetRolesAsync(user);
                var jwtSecurityTokenTask = GenerateJwtToken(user);

                authResponseModel.IsAuthenticated = true;
                authResponseModel.Email = user.Email;
                authResponseModel.UserName = user.UserName;
                authResponseModel.Roles = rolesList;
                authResponseModel.Token = new JwtSecurityTokenHandler().WriteToken(await jwtSecurityTokenTask);

                res.OK = true;
                res.Message = "Login Successfully";
                res.Data = authResponseModel;
            }

            return res;
        }

        protected override async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                //additional claims
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
