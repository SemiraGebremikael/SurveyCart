using Microsoft.AspNetCore.Identity;

namespace SurveyCart.Api.Entities
{
    public class UserRole: IdentityRole
    {
        public bool IsDefault { get; set; }
        public bool ISDeleted { get; set; } 

    }
}
