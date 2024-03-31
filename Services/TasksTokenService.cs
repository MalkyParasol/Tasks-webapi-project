using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MyTasks.Models;
using System.Text.Json;

namespace MyTasks.Services
{
    public static class TasksTokenService
    {
        private static byte[] keyBytes = new byte[32];
        private static List<Admin> admins;
        private static SymmetricSecurityKey key = new SymmetricSecurityKey(keyBytes);

        private static string issuer = "https://tasks-webapi-project.com";
        static TasksTokenService() 
        {
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(keyBytes);

            using var jsonFile = File.OpenText(Path.Combine("Data", "AdminsList.json"));
            admins = JsonSerializer.Deserialize<List<Admin>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
                ) ?? new List<Admin>();
            


        }
        public static List<Admin> GetAdmins()
        {
            return admins;
        }
       
        

        public static SecurityToken GetToken(List<Claim> claims) =>
           new JwtSecurityToken(
               issuer,
               issuer,
               claims,
               expires: DateTime.Now.AddMonths(4),
               signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
           );

           public static TokenValidationParameters GetTokenValidationParameters() =>
            new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero // remove delay of token when expire
            };


        public static string WriteToken(SecurityToken token) =>
           new JwtSecurityTokenHandler().WriteToken(token);
    }
}

