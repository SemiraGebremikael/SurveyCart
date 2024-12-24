using Microsoft.AspNetCore.Identity;

namespace SurveyCart.Api.Entities
{
    public sealed class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
