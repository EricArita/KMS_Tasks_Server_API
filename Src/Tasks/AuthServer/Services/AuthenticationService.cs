using AuthServer.Interfaces;
using AuthServer.Models;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Services
{
    public class AuthenticationService : JWTAuthenticationBase, IAuthentication
    {
        private readonly JWT _jwt;

        public AuthenticationService(IOptions<JWT> jwt)
        {
            _jwt = jwt.Value;
        }

        public async Task<HttpResponse<AuthResponseModel>> VerifyAccount(string userName, string password)
        {
            var authResponseModel = new AuthResponseModel();
            var res = new HttpResponse<AuthResponseModel>();

            // call UserManagement service instead
            //var user = await _userManager.FindByNameAsync(userName) ?? await _userManager.FindByEmailAsync(userName);

            var apiCaller = new HttpClient();
            var responseString = await apiCaller.GetStringAsync($"https://localhost:5005/api/v1/User/getuser/{userName}");
            var user = JsonConvert.DeserializeObject<HttpResponse<ApplicationUser>>(responseString).Data;

            if (user == null)
            {
                authResponseModel.IsAuthenticated = false;
                res.OK = false;
                res.Message = $"There is not any accounts registered with {userName}.";
                res.Data = authResponseModel;
                var a = new PasswordHasher<ApplicationUser>();
            }
            else if (new PasswordHasher<ApplicationUser>().VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                authResponseModel.IsAuthenticated = false;
                res.OK = false;
                res.Message = "Password is invalid";
                res.Data = authResponseModel;
            }
            else
            {
                res.Data = await GenerateUserWithAccessToken(user);
                res.OK = true;
                res.Message = "Login Successfully";
            }

            return res;
        }

        public async Task<HttpResponse<AuthResponseModel>> RefreshTokenAsync(string token)
        {
            var authResponseModel = new AuthResponseModel();
            var res = new HttpResponse<AuthResponseModel>();

            if (!String.IsNullOrEmpty(token))
            {
                //var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
                ApplicationUser user = null;
                if (user != null)
                {
                    var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
                    if (!refreshToken.IsExpired)
                    {
                        res.Data = await GenerateUserWithAccessToken(user, refreshToken);
                        res.OK = true;
                        return res;
                    }
                }
            }

            res.Message = "Refresh token invalid or expired! Need to sign in again";
            res.OK = false;
            return res;
        }

        protected override async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("https://localhost:5005/api/v1/User/get-user-claim/", data);
            string jsonResult = await result.Content.ReadAsStringAsync();
            var userClaims = JsonConvert.DeserializeObject<List<Claim>>(jsonResult);

            result = await client.PostAsync("https://localhost:5005/api/v1/User/get-user-role/", data);
            jsonResult = await result.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<string>>(jsonResult);

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
                new Claim("uid", user.UserId.ToString()),
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

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    ExpiredDate = DateTime.UtcNow.AddMinutes(5),
                    CreatedDate = DateTime.UtcNow
                };
            }
        }

        private async Task<AuthResponseModel> GenerateUserWithAccessToken(ApplicationUser user, RefreshToken activeRefreshToken = null)
        {
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            
            var result = await client.PostAsync("https://localhost:5005/api/v1/User/get-user-role/", data);
            string jsonResult = await result.Content.ReadAsStringAsync();

            var listRoles = JsonConvert.DeserializeObject<List<string>>(jsonResult);
            var jwtSecurityTokenTask = GenerateJwtToken(user);
            var authResponse = new AuthResponseModel();

            authResponse.IsAuthenticated = true;
            authResponse.Email = user.Email;
            authResponse.UserName = user.UserName;
            authResponse.Avatar = user.AvatarUrl;
            authResponse.Roles = listRoles;
            authResponse.Token = new JwtSecurityTokenHandler().WriteToken(await jwtSecurityTokenTask);

            if (activeRefreshToken != null)
            {
                authResponse.RefreshToken = activeRefreshToken.Token;
                authResponse.RefreshTokenExpiration = activeRefreshToken.ExpiredDate;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authResponse.RefreshToken = refreshToken.Token;
                authResponse.RefreshTokenExpiration = refreshToken.ExpiredDate;
                user.RefreshTokens.Add(refreshToken);
                //await _userManager.UpdateAsync(user);
            }

            return authResponse;
        }
    }
}