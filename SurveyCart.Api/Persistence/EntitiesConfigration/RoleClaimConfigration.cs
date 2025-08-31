

using Microsoft.AspNetCore.Identity;

namespace SurveyCart.Api.Persistence.EntitiesConfigration;
public class RoleClaimConfigration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {

        var permissions = Permissions.GetAllPermissions();
        var adminClaims = new List<IdentityRoleClaim<string>>();
        for(int i = 0; i < permissions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                RoleId = DefaultRoles.AdminRoleID,
                ClaimType = Permissions.Type,
                ClaimValue = permissions[i],
            });
        }
        builder.HasData(adminClaims); 

    }
}
