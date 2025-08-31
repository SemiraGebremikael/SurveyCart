using Microsoft.AspNetCore.Identity;

namespace SurveyCart.Api.Entities
{
    public sealed class Role: IdentityRole
    {
        public bool IsDefault { get; set; }
        public bool ISDeleted { get; set; } 

    }
}
