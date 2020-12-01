using Core.Application.Helper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.Constants;
using Infrastructure.Persistence.SettingModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class AuthenticationService : JWTAuthenticationBase, IAuthentication
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly FacebookAuthSettings _fbAuthSettings;

        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IOptions<FacebookAuthSettings> fbAuthSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _fbAuthSettings = fbAuthSettings.Value;
        }

        public async Task<Response<ApplicationUser>> RegisterAsync(UserRegisterModel model)
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
                    Status = 1
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
                    res.Errors = result.Errors.ToList();
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
                res = await GenerateUserWithTokenResponse(user);
            }

            return res;
        }

        public async Task<Response<AuthResponseModel>> HandleFacebookLoginAsync(string userAccessToken)
        {
            var client = new HttpClient();
            var authResponse = await client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessTokenResponse>(authResponse);

            var userAccessTokenValidationResponse = await client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={userAccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            { 
               return new Response<AuthResponseModel>(false, data: null, message:"User access token is not valid or has been expired");
            }

            var registeredUser = _userManager.Users.Where(e => e.LoginProvider == Enums.LoginProvider.Facebook.ToString() 
                                                            && e.ProviderKey == userAccessTokenValidation.Data.UserId).FirstOrDefault();
            if (registeredUser != null)
            {
                return await GenerateUserWithTokenResponse(registeredUser);
            }

            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v9.0/me?fields=id,email,name,first_name,middle_name,last_name,gender,birthday,picture&access_token={userAccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var user = new ApplicationUser
            {
                FirstName = userInfo.FirstName,
                MidName = userInfo.MiddleName,
                LastName = userInfo.LastName,
                Email = userInfo.Email,
                UserName = userInfo.Name,
                DateOfBirth = DateTime.Parse(userInfo.BirthDay),
                Gender = userInfo.Gender,
                AvatarUrl = userInfo.Picture.Data.Url,
                LoginProvider = Enums.LoginProvider.Facebook.ToString(),
                ProviderKey = userInfo.FacebookUserId,
                Status = 1
            };

            var res = await _userManager.CreateAsync(user);

            if (res.Succeeded)
            {
                return await GenerateUserWithTokenResponse(user);
            }

            return new Response<AuthResponseModel>(false, data: null, message: "Login fail. See list of errors for details", errors: res.Errors.ToList()) ;
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
                new Claim("uid", user.Id),
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

        private async Task<Response<AuthResponseModel>> GenerateUserWithTokenResponse(ApplicationUser user)
        {
            var listRoles = await _userManager.GetRolesAsync(user);
            var jwtSecurityTokenTask = GenerateJwtToken(user);
            var authResponse = new AuthResponseModel();
            var res = new Response<AuthResponseModel>();

            authResponse.IsAuthenticated = true;
            authResponse.Email = user.Email;
            authResponse.UserName = user.UserName;
            authResponse.Roles = listRoles;
            authResponse.Token = new JwtSecurityTokenHandler().WriteToken(await jwtSecurityTokenTask);

            res.OK = true;
            res.Message = "Login Successfully";
            res.Data = authResponse;

            return res;
        }
    }
}