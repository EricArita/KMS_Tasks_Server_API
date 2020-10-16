using Core.Application.Helper;
using Core.Application.Interfaces;
using Core.Application.Models;
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
    public class AuthenticationService : IAuthentication
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
        public override async Task<Response<AuthenticationResponseModel>> VerifyAccount(string userName, string password)
        {
            var authResponseModel = new AuthenticationResponseModel();
            var res = new Response<AuthenticationResponseModel>();

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
