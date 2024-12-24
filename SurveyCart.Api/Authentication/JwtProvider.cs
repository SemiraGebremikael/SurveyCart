
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyCart.Api.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        public (string token, int expiresIn) GenerateToken(User user)
        {
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ];

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("636f8080daeae435f1d7920f4b48d9327245dcad21ba42fdf1e85a334d56320b"));
            var signingCredentials = new SigningCredentials (symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var expiresIn = 30;
            var expirationDate = DateTime.UtcNow.AddMinutes(expiresIn);
            var token = new JwtSecurityToken(
                issuer: "SurveyCartApp",
                audience: "SurveyCartApp Users",
                claims: claims,
                signingCredentials: signingCredentials
                );
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn *60);
        }

    }
}
