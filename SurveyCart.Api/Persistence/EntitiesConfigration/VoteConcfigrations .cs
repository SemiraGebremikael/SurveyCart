
namespace SurveyCart.Api.Persistence.EntitiesConfigration;
public class VoteConcfigrations : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(x =>  new { x.PollId, x.UserId }).IsUnique();


    }
}
