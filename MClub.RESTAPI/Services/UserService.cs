using MClub.RESTAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MClub.RESTAPI.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }
    public class UserService : IUserService
    {
        public UserService()
        {

        }
        public async Task<User> Authenticate(string username, string password)
        {
            User user = null;

            var tokenJwtReponse = await GetAzureADAccessToken(username, password);

            if (tokenJwtReponse != null)
            {
                user = new User();
                var claims = GetFilteredUserClaims(tokenJwtReponse);
                if (claims.Any())
                {
                    user.Email = claims.Where(t => t.Type == "unique_name").FirstOrDefault()?.Value;
                    user.FirstName = claims.Where(t => t.Type == "name").FirstOrDefault()?.Value;
                    user.UserName = claims.Where(t => t.Type == "given_name").FirstOrDefault()?.Value;
                }

                // authentication successful so generate jwt token
                var accessToken = AccessToken(claims);
                user.AccessToken = accessToken;

            //    RefreshToken(accessToken, accessToken);
          
            }
            return user;
        }
        private async Task<JwtSecurityToken> GetAzureADAccessToken(string username, string password)
        {

            HttpClient httpClient = new HttpClient();
            try
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("resource", ConfigurationManager.AppSettings.Get("resource"));
                parameters.Add("client_id", ConfigurationManager.AppSettings.Get("client_id"));
                parameters.Add("client_secret", ConfigurationManager.AppSettings.Get("client_secret"));
                parameters.Add("grant_type", ConfigurationManager.AppSettings.Get("grant_type"));
                parameters.Add("scope", ConfigurationManager.AppSettings.Get("scope"));
                parameters.Add("username", username);
                parameters.Add("password", password);

                var encodedContent = new FormUrlEncodedContent(parameters);

                var httpResponseMessage = await httpClient.PostAsync(new Uri(ConfigurationManager.AppSettings.Get("TokenUrl")), encodedContent);
                string tokenJwtReponse = await httpResponseMessage.Content.ReadAsStringAsync();

                JObject ADToken = JObject.Parse(tokenJwtReponse);

                if (ADToken.HasValues)
                {
                    if (ADToken["error"] != null && ADToken["error_description"] != null)
                    {
                        throw new Exception(ADToken["error_description"].ToString().Split('.')[0]);
                    }
                    if (ADToken["access_token"] != null)
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var tokenS = handler.ReadToken(ADToken["access_token"].ToString()) as JwtSecurityToken;

                        return tokenS;
                    }

                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private IEnumerable<Claim> GetFilteredUserClaims(JwtSecurityToken tokenJwtReponse)
        {

            var filterClaims= tokenJwtReponse.Claims.
                Where(t => (t.Type == "name" || t.Type == "unique_name" || t.Type == "given_name")).ToList();

            filterClaims.Add(new Claim(ClaimTypes.Email, filterClaims.Where(t => t.Type == "unique_name").FirstOrDefault().Value));

            return filterClaims;
        }

        private string AccessToken(IEnumerable<Claim> claims)
        {
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings.Get("Secret"));
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddDays(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};


            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);


            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);

            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            string sec = ConfigurationManager.AppSettings.Get("Secret");
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            
            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: ConfigurationManager.AppSettings.Get("jwtTokenIssuer"), 
                                    audience: ConfigurationManager.AppSettings.Get("jwtTokeAudience"),
                                    subject: claimsIdentity, 
                                    notBefore: issuedAt, 
                                    expires: expires, 
                                    signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;

        }
    }
}