

using Microsoft.AspNetCore.Identity;

namespace SurveyCart.Api.Persistence.EntitiesConfigration;
public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {

        // dafault data
        builder.HasData([
            new IdentityUserRole<string>
            {
                UserId = DefaultUsers.AdminId,
                RoleId = DefaultRoles.AdminRoleID,
            },
        ]); 

    }
}
