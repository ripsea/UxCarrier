using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace UxCarrier.Helper
{
    public static class ExtensionMethods
    {
        public static string GetClaimName(this ClaimsPrincipal user)
        {
            var name = user.Claims
                .Where(x => x.Type == ClaimTypes.Name)
                .FirstOrDefault();

            if (name == null) { return string.Empty; }

            return user.Claims
                .Where(x => x.Type == ClaimTypes.Name)
                .FirstOrDefault()!
                .Value;
        }

        public static string GetClaimEmail(this ClaimsPrincipal user)
        {
            var name = user.Claims
                .Where(x => x.Type == ClaimTypes.Email)
                .FirstOrDefault();

            if (name == null) { return string.Empty; }

            return user.Claims
                .Where(x => x.Type == ClaimTypes.Email)
                .FirstOrDefault()!
                .Value;
        }

        public static string GetCardNo(this JwtSecurityToken tokenHandler)
        {
            //var token = new JwtSecurityTokenHandler().ReadJwtToken(dto.Token);
            return tokenHandler.Claims.First(c => c.Type == "unique_name").Value;
        }

        public static string GetEmail(this JwtSecurityToken tokenHandler)
        {
            //var token = new JwtSecurityTokenHandler().ReadJwtToken(dto.Token);
            return tokenHandler.Claims.First(c => c.Type == "email").Value;
        }

        //public static string GetClaimEmail(this ClaimsPrincipal user)
        //{
        //    return user.Claims
        //        .Where(x => x.Type == ClaimTypes.Email)
        //        .FirstOrDefault()!
        //        .Value;
        //}
    }
}
