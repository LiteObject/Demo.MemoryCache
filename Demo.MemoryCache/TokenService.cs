using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Demo.MemoryCache
{
    public static class TokenService
    {
        private const string Key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        public static string GetJwtToken(int expSeconds = 5)
        {
            string tokenString = string.Empty;

            try
            {
                SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Key));

                JwtSecurityTokenHandler handler = new();

                List<Claim> claims = new()
                {
                    new Claim("type2", "value2"),
                    new Claim("expiration", DateTime.UtcNow.AddSeconds(expSeconds).ToLongTimeString()),
                };

                string secToken1 = handler.CreateEncodedJwt(
                    issuer: "http://www.LiteObjects.com/",
                    audience: null,
                    subject: new ClaimsIdentity(claims),
                    notBefore: null,
                    issuedAt: DateTime.UtcNow, 
                    expires: DateTime.UtcNow.AddSeconds(expSeconds),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );

                // Console.WriteLine(secToken1);

                // Token to String so you can use it in your client
                // tokenString = handler.WriteToken(secToken);

                tokenString = secToken1;

                // handler.WriteToken(token) returns the token
                // Console.WriteLine(tokenString);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                Console.Error.WriteLine(new System.Diagnostics.StackTrace().ToString());
            }

            return tokenString;
        }

        private static string GetKeys()
        {
            /**************************************************************************
             * In .NET, the RSACryptoServiceProvider and DSACryptoServiceProvider
             * classes are used for asymmetric encryption (AKA public-key encryption)
             **************************************************************************/

            RSACryptoServiceProvider rsa = new();

            /**************************************************************************
             * If passed false, it returns public key only. If passed
             * true, it returns both private and public pair.
             **************************************************************************/

            string keyStr = rsa.ToXmlString(true);

            // Get key into parameters  
            /* RSAParameters RSAKeyInfo = RSA.ExportParameters(true);
            Console.WriteLine($"Modulus: {System.Text.Encoding.UTF8.GetString(RSAKeyInfo.Modulus)}");
            Console.WriteLine($"Exponent: {System.Text.Encoding.UTF8.GetString(RSAKeyInfo.Exponent)}");
            Console.WriteLine($"P: {System.Text.Encoding.UTF8.GetString(RSAKeyInfo.P)}");
            Console.WriteLine($"Q: {System.Text.Encoding.UTF8.GetString(RSAKeyInfo.Q)}");
            Console.WriteLine($"DP: {System.Text.Encoding.UTF8.GetString(RSAKeyInfo.DP)}");
            Console.WriteLine($"DQ: {System.Text.Encoding.UTF8.GetString(RSAKeyInfo.DQ)}"); */

            return keyStr;
        }
    }
}
