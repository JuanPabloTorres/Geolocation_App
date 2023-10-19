using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GeolocationAdsAPI.ApiTools
{
    public class JwtTool
    {
        public static string GenerateJSONWebToken(IConfiguration config, string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim(JwtRegisteredClaimNames.NameId, userId) };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddHours(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateJwtToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Example usage
            byte[] _generatedKey = GenerateSecureKey(32); // 32 bytes for HMACSHA256

            // Convert the key to a base64-encoded string for storage or transmission
            string base64Key = Convert.ToBase64String(_generatedKey);

            var key = Encoding.ASCII.GetBytes(base64Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // Generate a secure random key of a specific length (e.g., 32 bytes for HMACSHA256)
        public static byte[] GenerateSecureKey(int keyLength)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[keyLength];

                rng.GetBytes(key);

                return key;
            }
        }

        public static SymmetricSecurityKey GenerateSymmetricSecurityKey()
        {
            // Generate a random and secure key
            byte[] keyBytes = new byte[64]; // 64 bytes for a strong key

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }

            // Convert the byte array to a base64-encoded string
            string base64Key = Convert.ToBase64String(keyBytes);

            // Use the base64Key as the secret key for JWT token validation
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(base64Key));
        }
    }
}