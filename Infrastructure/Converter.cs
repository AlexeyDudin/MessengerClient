using System;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure
{
    public static class Converter
    {
        public static string DecodeJWTPayload(this string loginDto, string key) 
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(loginDto);
                return token.Payload[key].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
