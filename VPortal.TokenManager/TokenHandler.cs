using System;
using System.Text;
using Jose;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VPortal.TokenManager
{
    public class TokenHandler
    {
        public static string PrivateKey = "18b4bebb-8dd7-4b97-ac2c-16ceca5647f2";
        public static string TokenIssuerName = "Verscend";
        public static byte[] secretKey = Encoding.UTF8.GetBytes(PrivateKey);
        public static JwsAlgorithm Algorithm = JwsAlgorithm.HS512;

        public static Token GenerateToken(int userId)
        {
            var token = new Token();
            token.AddHeader("typ", "JWT");
            token.IssuedBy = TokenIssuerName;
            token.CreatedOn = DateTimeOffset.Now;
            token.ExpiresOn = DateTimeOffset.Now.AddHours(10);
            token.Subject = "JwtAuthentication";
            token.UserId = userId;
            token.ClientToken = Encode(token);
    
            return token;
        }
        private static string Encode(Token token)
        {
            return JWT.Encode(token.Claims, secretKey, Algorithm, token.Header);
        }
        public static Token Decode(string clientToken)
        {
            var token = new Token();
            string decoded = JWT.Decode(clientToken, secretKey);
            var dto = JsonConvert.DeserializeObject<Dictionary<string, string>>(decoded);
            token.ClientToken = clientToken;

            foreach (var c in dto)
            {
                switch (c.Key)
                {
                    case "iat":
                        token.CreatedOn = new DateTime(long.Parse(c.Value), DateTimeKind.Utc);
                        break;
                    case "exp":
                        token.ExpiresOn = new DateTime(long.Parse(c.Value), DateTimeKind.Utc);
                        break;
                    case "sub":
                        token.Subject = c.Value;
                        break;
                    case "iss":
                        token.IssuedBy = c.Value;
                        break;
                    case "userid":
                        token.UserId = int.Parse(c.Value);
                        break;
                }
            }
            return token;
        }

    }
}