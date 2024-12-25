using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace SurveyCart.Api.Entities
{
    public sealed class User:IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<RefreshToken> RefreshTokens { get; set; } =  [];
    }
}
