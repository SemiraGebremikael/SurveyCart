
using Microsoft.AspNetCore.Identity;

namespace SurveyCart.Api.Persistence.EntitiesConfigration;
public class UserConfigration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName)
               .HasMaxLength(100);
        builder.Property(x => x.LastName)
               .HasMaxLength(1000);
        builder.OwnsMany(x => x.RefreshTokens)
               .ToTable("RefreshTokens");

        // dafault admin user
        var PasswordHasher = new PasswordHasher<User>();
        builder.HasData(new User
        {
            Id = DefaultUsers.AdminId,
            FirstName = "Survey Cart",
            LastName = "Admine",
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            PasswordHash = PasswordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
        }); 

    }
}
