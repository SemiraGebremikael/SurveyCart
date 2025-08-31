
using Microsoft.AspNetCore.Identity;

namespace SurveyCart.Api.Persistence.EntitiesConfigration;
public class RoleConfigration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {


        builder.HasData([
            new Role
            {
                Id = DefaultRoles.AdminRoleID,
                Name = DefaultRoles.Admin,
                NormalizedName = DefaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
            },
            new  Role
            {
                Id = DefaultRoles.MemberRoleID,
                Name = DefaultRoles.Member,
                NormalizedName = DefaultRoles.Member.ToUpper(),
                ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
                IsDefault = true
            }
        ]);

    }
}
