using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MyTasks.Services
{
    public static class TasksTokenService
    {
        private static byte[] keyBytes = new byte[32];
        static TasksTokenService() // Use static constructor for initialization
        {
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(keyBytes);
        }
        private static SymmetricSecurityKey key = new SymmetricSecurityKey(keyBytes);

        private static string issuer = "https://tasks-webapi-project.com";

        public static SecurityToken GetToken(List<Claim> claims) =>
           new JwtSecurityToken(
               issuer,
               issuer,
               claims,
               expires: DateTime.Now.AddDays(30.0),
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

